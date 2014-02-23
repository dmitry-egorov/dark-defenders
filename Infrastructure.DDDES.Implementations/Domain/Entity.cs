using System;
using Microsoft.Practices.Unity;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class Entity<TEntity, TEvents> : IEntity<TEntity>, IEntityEvents
        where TEntity : IEntity<TEntity>, TEvents 
        where TEvents : IEntityEvents
    {
        [Dependency]public IEventsProcessor Processor { set { _processor = value; } }
        [Dependency]public TEvents External { set { _external = value; } }
        [Dependency]public IStorage<TEntity> Storage { set { _storage = value; } }

        private IEventsProcessor _processor;
        private TEvents _external;
        private IStorage<TEntity> _storage;

        private readonly IdentityOf<TEntity> _id;
        private readonly TEntity _entity;
        private bool _destroyed;

        protected Entity()
        {
            _id = new IdentityOf<TEntity>();
            _entity = (TEntity) (object) this;
        }

        public IdentityOf<TEntity> Id
        {
            get { return _id; }
        }

        protected void CreationEvent(Action<TEvents> action)
        {
            _processor.Publish(new Created<TEntity, TEvents>(_entity, _external, _storage, action));
        }

        protected void Event(Action<TEvents> action)
        {
            Action<TEvents> eventAction = x =>
            {
                if (!_destroyed)
                {
                    action(x);
                }
            };
            _processor.Publish(new Event<TEntity, TEvents>(_entity, _external, eventAction));
        }

        protected void DestructionEvent()
        {
            _processor.Publish(new Destroyed<TEntity, TEvents>(_entity, _external, _storage));
        }

        void IEntityEvents.Destroyed()
        {
            _destroyed = true;
        }
    }
}