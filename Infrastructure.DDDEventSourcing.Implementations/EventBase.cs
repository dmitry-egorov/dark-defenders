using Infrastructure.DDDEventSourcing.Domain;

namespace Infrastructure.DDDEventSourcing.Implementations
{
    public abstract class EventBase<TId> : IEvent
        where TId: Identity
    {
        protected EventBase(TId aggregateRootId)
        {
            AggregateRootId = aggregateRootId;
        }

        public TId AggregateRootId { get; private set; }
    }
}