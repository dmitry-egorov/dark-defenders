using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Physics;

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

        public void ApplyTo(IRigidBodyEventsReciever reciever)
        {
            reciever.Recieve(this);
        }

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}
