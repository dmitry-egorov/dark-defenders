using System;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class Created<TRoot, TId> : IEvent
        where TRoot: IEntity<TId>
    {
        private readonly Lazy<TRoot> _lazyRoot;
        private readonly IStorage<TRoot> _storage;

        protected Created(IStorage<TRoot> storage)
        {
            _lazyRoot = new Lazy<TRoot>(Create);
            _storage = storage;
        }

        public void Apply()
        {
            var entity = _lazyRoot.Value;
            _storage.Store(entity);
        }

        public object ToDto()
        {
            var rootId = _lazyRoot.Value.GetGlobalId();
            return CreateDto(rootId);
        }

        protected abstract object CreateDto(TId rootId);
        protected abstract TRoot Create();

        public override string ToString()
        {
            return "Root {0} created".FormatWith(typeof(TRoot).Name);
        }
    }
}