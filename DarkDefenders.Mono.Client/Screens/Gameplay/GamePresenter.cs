using System;
using System.Collections.Generic;
using DarkDefenders.Game.Model.Other;
using DarkDefenders.Game.Resources.Internals;
using DarkDefenders.Remote.Model;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkDefenders.Mono.Client.Screens.Gameplay
{
    public class GamePresenter : IRemoteEvents
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Texture2D _groundTexture;
        private Map<Tile> _map;

        private volatile bool _loaded;
        private readonly SpriteBatch _spriteBatch;

        private readonly Dictionary<IdentityOf<RemoteEntity>, Entity> _entities = new Dictionary<IdentityOf<RemoteEntity>, Entity>();

        public GamePresenter(GraphicsDevice graphicsDevice, Texture2D groundTexture)
        {
            _graphicsDevice = graphicsDevice;
            _groundTexture = groundTexture;
            _spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public void MapLoaded(string mapId)
        {
            var data = WorldLoader.LoadFromFile(mapId, "Content");

            _map = data.Map;

            _loaded = true;
        }

        public void Destroyed(IdentityOf<RemoteEntity> id)
        {
            _entities.Remove(id);
        }

        public void Moved(IdentityOf<RemoteEntity> id, Vector newPosition)
        {
            Entity entity;
            if (_entities.TryGetValue(id, out entity))
            {
                entity.Position = newPosition;
            }
        }

        public void Created(IdentityOf<RemoteEntity> id, Vector initialPosition, RemoteEntityType type)
        {
            _entities[id] = new Entity(initialPosition, type);
        }

        public void Present()
        {
            if (!_loaded)
            {
                return;
            }
            
//            _effect.CurrentTechnique.Passes[0].Apply();
//
//            _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _mapVertices, 0, _mapVertices.Length / 3);
            var projection = CreateProjectionMatrix();

            _spriteBatch.Begin(0, null, null, null, RasterizerState.CullClockwise, null, projection);

            DrawTerrain();
            DrawEntities();

            _spriteBatch.End();
        }

        private Matrix CreateProjectionMatrix()
        {
            var viewport = _graphicsDevice.Viewport;

            return Matrix.CreateTranslation(-50f, -40f, 0)
                 * Matrix.CreateScale(8000.0f / viewport.Height)
                 * Matrix.CreateScale(1, -1, 1)
                 * Matrix.CreateTranslation(viewport.Width / 2.0f, viewport.Height / 2.0f, 0.0f);
        }

        private void DrawTerrain()
        {
            for (var x = 0; x < _map.Dimensions.Width; x++)
            {
                for (var y = 0; y < _map.Dimensions.Height; y++)
                {
                    var color = GetColor(_map, x, y);

                    _spriteBatch.Draw(_groundTexture, new Rectangle(x, y, 1, 1), color);
                }
            }
        }

        private void DrawEntities()
        {
            foreach (var entity in _entities.Values)
            {
                var position = entity.Position;
                var color = entity.GetColor();
                _spriteBatch.Draw(_groundTexture, new Vector2(position.X.ToSingle() - 0.5f, position.Y.ToSingle() - 0.5f), new Rectangle(0, 0, 1, 1), color);
            }
        }

        private Color GetColor(Map<Tile> map, int x, int y)
        {
            var color = Color.SaddleBrown;
            var count = 0;

            if (map[x, y] == Tile.Open)
            {
                return new Color(150, 130, 86);
            }

            for (var i = x - 1; i <= x + 1; i++)
            for (var j = y - 1; j <= y + 1; j++)
            {
                if (i == x && j == y)
                {
                    continue;
                }

                if (map[i, j] == Tile.Solid)
                {
                    continue;
                }

                count++;

                if (count == 2)
                {
                    return color;
                }
            }

            return count == 0 ? Color.Black : color * 0.6f;
        }

        private class Entity
        {
            public Vector Position { get; set; }
            public RemoteEntityType Type { get; private set; }

            public Entity(Vector position, RemoteEntityType type)
            {
                Position = position;
                Type = type;
            }

            public Color GetColor()
            {
                switch (Type)
                {
                    case RemoteEntityType.Player:
                        return Color.Blue;
                    case RemoteEntityType.Hero:
                        return Color.BlueViolet;
                    case RemoteEntityType.Projectile:
                        return Color.Purple;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}