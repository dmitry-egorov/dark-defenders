using DarkDefenders.Domain.Players;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies.Events
{
    public class Accelerated : EventBase<RigidBodyId, Accelerated>, IRigidBodyEvent
    {
        public Vector NewMomentum { get; private set; }

        public Accelerated(RigidBodyId id, Vector newMomentum)
            : base(id)
        {
            NewMomentum = newMomentum;
        }

        protected override string EventToString()
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
            reciever.Apply(this);
        }
    }
}