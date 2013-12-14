using Infrastructure.DDDEventSourcing.Domain;

namespace Infrastructure.DDDEventSourcing.Implementations.Domain
{
    public abstract class EventBase<TRootId> : IEventMarker
        where TRootId: Identity
    {
        protected EventBase(TRootId rootId)
        {
            RootId = rootId;
        }

        public TRootId RootId { get; private set; }
    }
}