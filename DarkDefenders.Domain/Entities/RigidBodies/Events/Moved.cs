using DarkDefenders.Domain.Data.Entities.RigidBodies;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class Moved : Event<RigidBody>
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

        protected override object CreateData(IdentityOf<RigidBody> id)
        {
            return new MovedData(id, _newPosition.ToData());
        }
    }
}