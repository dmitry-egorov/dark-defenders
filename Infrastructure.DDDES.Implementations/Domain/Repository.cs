using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class Repository<TRoot, TEvent, TEventReciever, TRootId> : IRepository<TRoot, TRootId>
        where TRoot: IRoot<TEvent> 
        where TEvent : IRootEvent<TRootId, TEventReciever>
        where TRootId: Identity
    {
        private readonly IEventStore _eventStore;
        private readonly Func<TRootId, TRoot> _factory;

        public Repository(IEventStore eventStore, Func<TRootId, TRoot> factory)
        {
            _eventStore = eventStore;
            _factory = factory;
        }

        public TRoot GetById(TRootId id)
        {
            var events = _eventStore.GetById(id).Cast<TEvent>();

            return CreateRootFrom(id, events);
        }

        public IEnumerable<TRoot> GetAll()
        {
            return _eventStore
                    .GetAll()
                    .OfType<TEvent>()
                    .GroupBy(x => x.RootId)
                    .Select(x => CreateRootFrom(x.Key, x));
        }

        private TRoot CreateRootFrom(TRootId id, IEnumerable<TEvent> events)
        {
            var root = _factory(id);

            root.Apply(events);

            return root;
        }
    }
}