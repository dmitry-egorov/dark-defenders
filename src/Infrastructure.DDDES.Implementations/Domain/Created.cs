using System;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public sealed class Created<TEntity, TEntityEvents> : IEvent
        where TEntity : IEntity<TEntity>, TEntityEvents
    {
        private readonly TEntity _entity;
        private readonly TEntityEvents _externalReciever;
        private readonly IStorage<TEntity> _storage;
        private readonly Action<TEntityEvents> _eventAction;

        public Created(TEntity entity, TEntityEvents externalReciever, IStorage<TEntity> storage, Action<TEntityEvents> eventAction)
        {
            _entity = entity;
            _storage = storage;
            _eventAction = eventAction;
            _externalReciever = externalReciever;
        }

        public void Apply()
        {
            _storage.Store(_entity);

            _eventAction(_entity);
            _eventAction(_externalReciever);
        }

        public override string ToString()
        {
            return "Entity {0}:{1} created".FormatWith(typeof(TEntity).Name, _entity.Id);
        }
    }
}