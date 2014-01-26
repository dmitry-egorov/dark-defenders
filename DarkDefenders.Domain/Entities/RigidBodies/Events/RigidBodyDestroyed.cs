using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Dtos.Entities.RigidBodies;
using DarkDefenders.Dtos.Infrastructure;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class RigidBodyDestroyed : Destroyed<RigidBody, RigidBodyId>
    {
        public RigidBodyDestroyed(RigidBody root, IStorage<RigidBody> storage) : base(root, storage)
        {
        }

        protected override IEventDto CreateDto(RigidBodyId id)
        {
            return new RigidBodyDestroyedDto(id);
        }
    }
}