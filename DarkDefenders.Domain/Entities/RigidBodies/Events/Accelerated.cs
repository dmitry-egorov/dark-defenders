using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Dtos.Entities.RigidBodies;
using DarkDefenders.Dtos.Infrastructure;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class Accelerated : DomainEvent<RigidBody, RigidBodyId>
    {
        private readonly Momentum _newMomentum;

        public Accelerated(RigidBody rigidBody, Momentum newMomentum) : base(rigidBody)
        {
            _newMomentum = newMomentum;
        }

        protected override void Apply(RigidBody rigidBody)
        {
            rigidBody.SetNewMomentum(_newMomentum);
        }

        protected override IEventDto CreateDto(RigidBodyId id)
        {
            return new AcceleratedDto(id, _newMomentum);
        }
    }
}