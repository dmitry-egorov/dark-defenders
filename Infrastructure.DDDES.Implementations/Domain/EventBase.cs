namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class EventBase<TRootId, TEvent> : IEvent
        where TRootId: Identity
        where TEvent: EventBase<TRootId, TEvent>
    {
        protected EventBase(TRootId rootId)
        {
            RootId = rootId;
        }

        Identity IEvent.RootId { get { return RootId; } }

        public TRootId RootId { get; private set; }

        public override string ToString()
        {
            return EventToString();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TEvent) obj);
        }

        public override int GetHashCode()
        {
            return (GetEventHashCode()*397) ^ RootId.GetHashCode();
        }

        protected abstract string EventToString();
        protected abstract bool EventEquals(TEvent other);
        protected abstract int GetEventHashCode();

        private bool Equals(TEvent other)
        {
            return RootId.Equals(other.RootId) && EventEquals(other);
        }
    }
}