using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public class Repository<TRoot, TSnapshot, TEvent, TEventReciever, TRootId> : IRepository<TRoot, TRootId>
        where TRoot: IRoot<TSnapshot>
        where TSnapshot: TEventReciever, IRootSnapshot<TRootId>
        where TRootId: Identity
        where TEvent : IRootEvent<TEventReciever>
    {
        private readonly IEventStore _eventStore;
        private readonly Func<TRoot> _factory;

        public Repository(IEventStore eventStore, Func<TRoot> factory)
        {
            _eventStore = eventStore;
            _factory = factory;
        }

        public TRoot GetById(TRootId id)
        {
            var events = _eventStore.GetById(id).Cast<TEvent>();

            return CreateRootFrom(events);
        }

        public IEnumerable<TRoot> GetAll()
        {
            return _eventStore
                    .GetAll()
                    .OfType<TEvent>()
                    .GroupBy(x => x.RootId)
                    .Select(CreateRootFrom);
        }

        private TRoot CreateRootFrom(IEnumerable<TEvent> events)
        {
            var root = _factory();

            events.ForEach(@event => @event.ApplyTo(root.Snapshot));

            return root;
        }
    }
}