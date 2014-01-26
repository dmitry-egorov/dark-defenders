using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Dtos.Entities.RigidBodies;
using DarkDefenders.Dtos.Infrastructure;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class Moved : DomainEvent<RigidBody, RigidBodyId>
    {
        private readonly Vector _newPosition;

        public Moved(RigidBody rigidBody, Vector newPosition) : base(rigidBody)
        {
            _newPosition = newPosition;
        }

        protected override void Apply(RigidBody rigidBody)
        {
            rigidBody.SetNewPosition(_newPosition);
        }

        protected override IEventDto CreateDto(RigidBodyId id)
        {
            return new MovedDto(id, _newPosition);
        }
    }
}