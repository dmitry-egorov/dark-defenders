using System;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public sealed class Event<TEntity, TEvents> : IEvent
        where TEntity : IEntity<TEntity>, TEvents
    {
        private readonly TEvents _entity;
        private readonly TEvents _external;
        private readonly Action<TEvents> _eventAction;

        public Event(TEvents entity, TEvents external, Action<TEvents> eventAction)
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