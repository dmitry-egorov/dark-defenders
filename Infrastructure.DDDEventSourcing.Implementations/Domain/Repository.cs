using System.Linq;
using Infrastructure.DDDEventSourcing.Domain;
using MoreLinq;

namespace Infrastructure.DDDEventSourcing.Implementations.Domain
{
    public class Repository<TRoot, TState, TEvent, TEventReciever, TRootId> : IRepository<TRoot, TRootId>
        where TRoot: IRoot<TState>, new()
        where TState: TEventReciever
        where TRootId: Identity
        where TEvent : IRootEvent<TEventReciever>
    {
        private readonly IEventStore _eventStore;

        public Repository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public TRoot GetById(TRootId id)
        {
            var events = _eventStore.Get(id).Cast<TEvent>();

            var root = new TRoot();

            events.ForEach(@event => @event.ApplyTo(root.State));

            return root;
        }
    }
}