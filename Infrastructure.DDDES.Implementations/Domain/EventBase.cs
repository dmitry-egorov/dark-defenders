using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class EventBase<TRootId, TEvent> : SlowValueObject<TEvent>, IEvent
        where TRootId: Identity
        where TEvent: EventBase<TRootId, TEvent>
    {
        protected EventBase(TRootId rootId)
        {
            RootId = rootId;
        }

        Identity IEvent.RootId { get { return RootId; } }

        public TRootId RootId { get; private set; }
    }
}