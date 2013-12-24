using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.RigidBodies.Events
{
    public class RigidBodyDestroyed : Destroyed<RigidBodyId, RigidBody, RigidBodyDestroyed>, IDomainEvent
    {
        public RigidBodyDestroyed(RigidBodyId rootId) : base(rootId)
        {
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}