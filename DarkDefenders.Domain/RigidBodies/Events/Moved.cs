using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies.Events
{
    public class Moved : EventBase<RigidBodyId, Moved>, IRigidBodyEvent
    {
        public Vector NewPosition { get; private set; }

        public Moved(RigidBodyId id, Vector newPosition): base(id)
        {
            NewPosition = newPosition;
        }

        public void ApplyTo(IRigidBodyEventsReciever reciever)
        {
            reciever.Recieve(this);
        }

        protected override string ToStringInternal()
        {
            return "RigidBody moved: {0} {1}".FormatWith(RootId, NewPosition);
        }

        protected override bool EventEquals(Moved other)
        {
            return NewPosition.Equals(other.NewPosition);
        }

        protected override int GetEventHashCode()
        {
            return NewPosition.GetHashCode();
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}