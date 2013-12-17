using System;
using System.Collections.Generic;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class Repository<TRootId, TRoot> : IRepository<TRootId, TRoot> 
        where TRootId: Identity
    {
        private readonly Dictionary<TRootId, TRoot> _roots = new Dictionary<TRootId, TRoot>();

        private readonly Func<TRootId, TRoot> _factory;

        public Repository(Func<TRootId, TRoot> factory)
        {
            _factory = factory;
        }

        public TRoot GetById(TRootId id)
        {
            return _roots.GetOrCreate(id, () => _factory(id));
        }

        public IEnumerable<TRoot> GetAll()
        {
            return _roots.Values;
        }
    }
}