using System;
using System.Collections.Generic;
using DarkDefenders.Remote.Model;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkDefenders.Mono.Client.Presenters
{
    public class AllEntitiesPresenter
    {
        private readonly Dictionary<IdentityOf<RemoteEntity>, EntityPresenter> _entities = new Dictionary<IdentityOf<RemoteEntity>, EntityPresenter>();
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _groundTexture;

        public AllEntitiesPresenter(SpriteBatch spriteBatch, Texture2D groundTexture)
        {
            _spriteBatch = spriteBatch;
            _groundTexture = groundTexture;
        }

        public void Remove(IdentityOf<RemoteEntity> id)
        {
            _entities.Remove(id);
        }

        public void ChangePosition(IdentityOf<RemoteEntity> id, Vector newPosition)
        {
            EntityPresenter entity;
            if (_entities.TryGetValue(id, out entity))
            {
                entity.SetPosition(newPosition);
            }
        }

        public void CreateNewEntity(IdentityOf<RemoteEntity> id, Vector initialPosition, RemoteEntityType type)
        {
            _entities[id] = new EntityPresenter(initialPosition, _spriteBatch, _groundTexture, GetEntityColor(type));
        }

        public void DrawEntities()
        {
            foreach (var entity in _entities.Values)
            {
                entity.Draw();
            }
        }

        private static Color GetEntityColor(RemoteEntityType type)
        {
            switch (type)
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