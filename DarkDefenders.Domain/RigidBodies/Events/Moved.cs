using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

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

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}