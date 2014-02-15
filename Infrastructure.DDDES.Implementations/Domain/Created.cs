using System;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class Created<TEntity, TReciever> : IEvent, IAcceptorOf<TReciever> 
        where TEntity : IEntity<TEntity>
    {
        private readonly Lazy<TEntity> _lazyEntity;
        private readonly IStorage<TEntity> _storage;

        protected Created(IStorage<TEntity> storage)
        {
            _lazyEntity = new Lazy<TEntity>(Create);
            _storage = storage;
        }

        public void Apply()
        {
            var entity = _lazyEntity.Value;

            _storage.Store(entity);
        }

        public void Accept(TReciever reciever)
        {
            Accept(reciever, _lazyEntity.Value.Id);
        }

        public override string ToString()
        {
            return "Entity {0}:{1} created".FormatWith(typeof(TEntity).Name, _lazyEntity.Value.Id);
        }

        protected abstract TEntity Create();
        protected abstract void Accept(TReciever reciever, IdentityOf<TEntity> id);
    }
}