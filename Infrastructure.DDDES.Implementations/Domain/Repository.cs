using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Util;
using MoreLinq;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class Repository<TRootId, TRoot, TEvent, TEventReciever> : IRootsStorage<TRootId, TRoot, TEvent> 
        where TRoot: IRoot<TEvent> 
        where TEvent : IRootEvent<TRootId, TEventReciever>
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

        public void Apply(IEnumerable<TEvent> events)
        {
            events
                .GroupBy(x => x.RootId)
                .ForEach(x => _roots[x.Key].Apply(x));
        }
    }
}