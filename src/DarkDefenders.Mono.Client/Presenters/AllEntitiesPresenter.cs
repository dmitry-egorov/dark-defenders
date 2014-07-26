using System;
using System.Collections.Generic;
using DarkDefenders.Remote.Model;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Microsoft.Xna.Framework.Graphics;

namespace DarkDefenders.Mono.Client.Presenters
{
    public class AllEntitiesPresenter
    {
        private readonly Dictionary<IdentityOf<RemoteEntity>, EntityPresenter> _presenters = new Dictionary<IdentityOf<RemoteEntity>, EntityPresenter>();
        private readonly IResources<RemoteEntityType, EntityProperties> _resources;
        private readonly SpriteBatch _spriteBatch;

        public AllEntitiesPresenter(SpriteBatch spriteBatch, IResources<RemoteEntityType, EntityProperties> resources)
        {
            _spriteBatch = spriteBatch;
            _resources = resources;
        }

        public void Remove(IdentityOf<RemoteEntity> id)
        {
            _presenters.Remove(id);
        }

        public void ChangePosition(IdentityOf<RemoteEntity> id, Vector newPosition)
        {
            TryApply(id, e => e.SetPosition(newPosition));
        }

        private void TryApply(IdentityOf<RemoteEntity> id, Action<EntityPresenter> action)
        {
            EntityPresenter entity;
            if (_presenters.TryGetValue(id, out entity))
            {
                action(entity);
            }
        }

        public void ChangeDirection(IdentityOf<RemoteEntity> id, Direction newDirection)
        {
            TryApply(id, e => e.SetDirection(newDirection));
        }

        public void CreateNewEntity(IdentityOf<RemoteEntity> id, Vector initialPosition, RemoteEntityType type)
        {
            _presenters[id] = CreateEntityPresenter(initialPosition, type);
        }

        public void Draw()
        {
            foreach (var entity in _presenters.Values)
            {
                entity.Draw();
            }
        }

        private EntityPresenter CreateEntityPresenter(Vector initialPosition, RemoteEntityType type)
        {
            return new EntityPresenter(_spriteBatch, initialPosition, _resources[type]);
        }
    }
}