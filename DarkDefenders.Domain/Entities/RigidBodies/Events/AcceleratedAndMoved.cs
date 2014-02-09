using DarkDefenders.Domain.Data.Entities.RigidBodies;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class AcceleratedAndMoved : Event<RigidBody>
    {
        private readonly Vector _newPosition;
        private readonly Momentum _newMomentum;

        public AcceleratedAndMoved(RigidBody rigidBody, Momentum newMomentum, Vector newPosition) 
            : base(rigidBody)
        {
            _newMomentum = newMomentum;
            _newPosition = newPosition;
        }

        protected override void Apply(RigidBody rigidBody)
        {
            rigidBody.SetNewMomentum(_newMomentum);
            rigidBody.SetNewPosition(_newPosition);
        }

        protected override object CreateData(IdentityOf<RigidBody> id)
        {
            return new AcceleratedAndMovedData(id, _newPosition.ToData(), _newMomentum.ToData());
        }
    }
}