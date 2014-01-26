using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Dtos.Entities.RigidBodies;
using DarkDefenders.Dtos.Infrastructure;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class ExternalForceChanged : DomainEvent<RigidBody, RigidBodyId>
    {
        private readonly Force _externalForce;

        public ExternalForceChanged(RigidBody rigidBody, Force externalForce) : base(rigidBody)
        {
            _externalForce = externalForce;
        }

        protected override void Apply(RigidBody rigidBody)
        {
            rigidBody.SetExternalForce(_externalForce);
        }

        protected override IEventDto CreateDto(RigidBodyId id)
        {
            return new ExternalForceChangedDto(id, _externalForce);
        }
    }
}
