using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.RigidBodies.Events
{
    public class RigidBodyRemoved : Removed<RigidBodyId, RigidBodyRemoved>, IDomainEvent
    {
        public RigidBodyRemoved(RigidBodyId rootId) : base(rootId)
        {
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}