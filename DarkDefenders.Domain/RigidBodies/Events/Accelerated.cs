using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math.Physics;
using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies.Events
{
    public class Accelerated : EventBase<RigidBodyId, Accelerated>, IRigidBodyEvent
    {
        public Momentum NewMomentum { get; private set; }

        public Accelerated(RigidBodyId id, Momentum newMomentum)
            : base(id)
        {
            NewMomentum = newMomentum;
        }

        protected override string ToStringInternal()
        {
            return "RigidBody accelerated: {0} {1}".FormatWith(RootId, NewMomentum);
        }

        protected override bool EventEquals(Accelerated other)
        {
            return NewMomentum.Equals(other.NewMomentum);
        }

        protected override int GetEventHashCode()
        {
            return NewMomentum.GetHashCode();
        }

        public void ApplyTo(IRigidBodyEventsReciever reciever)
        {
            reciever.Recieve(this);
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}