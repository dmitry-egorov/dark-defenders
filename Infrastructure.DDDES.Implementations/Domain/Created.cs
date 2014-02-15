using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class Created<TEntity, TReciever> : IEvent, IAcceptorOf<TReciever>
        where TEntity : IEntity<TEntity>
    {
        private readonly TEntity _entity;
        private readonly IStorage<TEntity> _storage;

        protected Created(TEntity entity, IStorage<TEntity> storage)
        {
            _entity = entity;
            _storage = storage;
        }

        public void Apply()
        {
            _storage.Store(_entity);
            ApplyTo(_entity);
        }

        public override string ToString()
        {
            return "Entity {0}:{1} created".FormatWith(typeof(TEntity).Name, _entity.Id);
        }

        protected abstract void ApplyTo(TEntity entity);

        public abstract void Accept(TReciever reciever);
    }
}