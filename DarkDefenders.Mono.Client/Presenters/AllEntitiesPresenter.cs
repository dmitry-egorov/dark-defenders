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
        private readonly Dictionary<IdentityOf<RemoteEntity>, EntityPresenter> _presenters = new Dictionary<IdentityOf<RemoteEntity>, EntityPresenter>();
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _whiteTexture;

        public AllEntitiesPresenter(SpriteBatch spriteBatch, Texture2D whiteTexture)
        {
            _spriteBatch = spriteBatch;
            _whiteTexture = whiteTexture;
        }

        public void Remove(IdentityOf<RemoteEntity> id)
        {
            _presenters.Remove(id);
        }

        public void ChangePosition(IdentityOf<RemoteEntity> id, Vector newPosition)
        {
            EntityPresenter entity;
            if (_presenters.TryGetValue(id, out entity))
            {
                entity.SetPosition(newPosition);
            }
        }

        public void CreateNewEntity(IdentityOf<RemoteEntity> id, Vector initialPosition, RemoteEntityType type)
        {
            _presenters[id] = CreateEntityPresenter(initialPosition, type);
        }

        public void DrawEntities()
        {
            foreach (var entity in _presenters.Values)
            {
                entity.Draw();
            }
        }

        private EntityPresenter CreateEntityPresenter(Vector initialPosition, RemoteEntityType type)
        {
            return new EntityPresenter(initialPosition, _spriteBatch, _whiteTexture, GetEntityColor(type));
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