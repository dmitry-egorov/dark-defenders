using DarkDefenders.Domain.Data.Entities.RigidBodies;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class RigidBodyDestroyed : Destroyed<RigidBody>
    {
        public RigidBodyDestroyed(RigidBody root, IStorage<RigidBody> storage) : base(root, storage)
        {
        }

        protected override object CreateData(IdentityOf<RigidBody> id)
        {
            return new RigidBodyDestroyedData(id);
        }
    }
}