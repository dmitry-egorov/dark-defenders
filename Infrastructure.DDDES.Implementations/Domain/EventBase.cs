using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class EventBase<TRootId, TEvent> : ValueObject<TEvent>, IEvent<TRootId>
        where TRootId: Identity
        where TEvent: EventBase<TRootId, TEvent>
    {
        protected EventBase(TRootId rootId)
        {
            RootId = rootId;
        }

        Identity IEvent.RootId { get { return RootId; } }

        public TRootId RootId { get; private set; }

        protected override int GetHashCodeInternal()
        {
            return (GetEventHashCode()*397) ^ RootId.GetHashCode();
        }

        protected override bool EqualsInternal(TEvent other)
        {
            return RootId.Equals(other.RootId) 
                   && EventEquals(other);
        }

        protected abstract bool EventEquals(TEvent other);
        protected abstract int GetEventHashCode();
    }
}