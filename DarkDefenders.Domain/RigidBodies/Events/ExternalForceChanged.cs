using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Math.Physics;
using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies.Events
{
    public class ExternalForceChanged : EventBase<RigidBodyId, ExternalForceChanged>, IRigidBodyEvent
    {
        public Force ExternalForce { get; private set; }

        public ExternalForceChanged(RigidBodyId rigidBodyId, Force externalForce)
            : base(rigidBodyId)
        {
            ExternalForce = externalForce;
        }

        protected override string ToStringInternal()
        {
            return "External force changed: {0}, {1}".FormatWith(RootId, ExternalForce);
        }

        protected override bool EventEquals(ExternalForceChanged other)
        {
            return ExternalForce.Equals(other.ExternalForce);
        }

        protected override int GetEventHashCode()
        {
            return ExternalForce.GetHashCode();
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
