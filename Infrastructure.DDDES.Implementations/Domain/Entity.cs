using System;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class Entity<TEntity, TEvents> : IEntity<TEntity> 
        where TEntity : IEntity<TEntity>, TEvents where TEvents : IEntityEvents
    {
        private readonly IdentityOf<TEntity> _id;
        private readonly TEvents _external;
        private readonly IStorage<TEntity> _storage;
        private readonly TEntity _entity;

        protected Entity(TEvents external, IStorage<TEntity> storage)
        {
            _external = external;
            _storage = storage;
            _id = new IdentityOf<TEntity>();
            _entity = (TEntity) (object) this;
        }

        public IdentityOf<TEntity> Id
        {
            get { return _id; }
        }

        protected IEvent CreationEvent(Action<TEvents> action)
        {
            return new Created<TEntity, TEvents>(_entity, _external, _storage, action);
        }

        protected IEvent Event(Action<TEvents> action)
        {
            return new Event<TEntity, TEvents>(_entity, _external, action);
        }

        protected IEvent DestructionEvent()
        {
            return new Destroyed<TEntity, TEvents>(_entity, _external, _storage);
        }
    }
}