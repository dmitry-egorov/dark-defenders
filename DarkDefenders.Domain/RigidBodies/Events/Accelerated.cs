using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Physics;

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