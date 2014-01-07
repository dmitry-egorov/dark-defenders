using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.RigidBodies.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.RigidBodies
{
    public class AcceleratedAndMoved: EventBase<RigidBodyId, AcceleratedAndMoved>, IRigidBodyEvent
    {
        public Momentum NewMomentum { get; private set; }
        public Vector NewPosition { get; private set; }

        public AcceleratedAndMoved(RigidBodyId id, Momentum newMomentum, Vector newPosition)
            : base(id)
        {
            NewMomentum = newMomentum;
            NewPosition = newPosition;
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