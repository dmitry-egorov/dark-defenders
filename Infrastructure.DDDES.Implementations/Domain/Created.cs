using System;
using Infrastructure.Data;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class Created<TEntity> : IEvent
        where TEntity : IEntity<TEntity>
    {
        private readonly Lazy<TEntity> _lazyRoot;
        private readonly IStorage<TEntity> _storage;

        protected Created(IStorage<TEntity> storage)
        {
            _lazyRoot = new Lazy<TEntity>(Create);
            _storage = storage;
        }

        public void Apply()
        {
            var entity = _lazyRoot.Value;

            _storage.Store(entity);
        }

        public object GetData()
        {
            var id = _lazyRoot.Value.Id;
            return CreateData(id);
        }

        public override string ToString()
        {
            return "Root {0} created".FormatWith(typeof(TEntity).Name);
        }

        protected abstract TEntity Create();

        protected abstract object CreateData(IdentityOf<TEntity> id);
    }
}