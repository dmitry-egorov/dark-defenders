using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Dtos.Entities.RigidBodies;
using DarkDefenders.Dtos.Infrastructure;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class AcceleratedAndMoved : DomainEvent<RigidBody, RigidBodyId>
    {
        private readonly Vector _newPosition;
        private readonly Momentum _newMomentum;

        public AcceleratedAndMoved(RigidBody rigidBody, Momentum newMomentum, Vector newPosition) : base(rigidBody)
        {
            _newMomentum = newMomentum;
            _newPosition = newPosition;
        }

        protected override void Apply(RigidBody rigidBody)
        {
            rigidBody.SetNewMomentum(_newMomentum);
            rigidBody.SetNewPosition(_newPosition);
        }

        protected override IEventDto CreateDto(RigidBodyId id)
        {
            return new AcceleratedAndMovedDto(id, _newPosition, _newMomentum);
        }
    }
}