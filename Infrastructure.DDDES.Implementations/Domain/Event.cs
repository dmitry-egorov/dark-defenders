using System;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public sealed class Event<TEntity, TEntityEvents> : IEvent
        where TEntity : IEntity<TEntity>, TEntityEvents
    {
        private readonly TEntityEvents _entity;
        private readonly TEntityEvents _external;
        private readonly Action<TEntityEvents> _eventAction;

        public Event(TEntityEvents entity, TEntityEvents external, Action<TEntityEvents> eventAction)
        {
            _entity = entity;
            _external = external;
            _eventAction = eventAction;
        }

        public void Apply()
        {
            _eventAction(_entity);
            _eventAction(_external);
        }
    }
}