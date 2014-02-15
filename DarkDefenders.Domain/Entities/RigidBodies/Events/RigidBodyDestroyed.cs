using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class RigidBodyDestroyed : Destroyed<RigidBody>
    {
        public RigidBodyDestroyed(RigidBody entity, IStorage<RigidBody> storage) 
            : base(entity, storage)
        {
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<RigidBody> id)
        {
            reciever.RigidBodyDestroyed(id);
        }
    }
}