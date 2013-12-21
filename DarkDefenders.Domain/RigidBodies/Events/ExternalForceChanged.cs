using DarkDefenders.Domain.Players;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies.Events
{
    public class ExternalForceChanged : EventBase<RigidBodyId, ExternalForceChanged>, IRigidBodyEvent
    {
        public Vector ExternalForce { get; private set; }

        public ExternalForceChanged(RigidBodyId rigidBodyId, Vector externalForce): base(rigidBodyId)
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
            reciever.Apply(this);
        }
    }
}
