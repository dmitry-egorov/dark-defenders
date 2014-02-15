using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.RigidBodies.Events
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