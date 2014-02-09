using DarkDefenders.Domain.Data.Entities.RigidBodies;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class Accelerated : Event<RigidBody>
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

        protected override object CreateData(IdentityOf<RigidBody> id)
        {
            return new AcceleratedData(id, _newMomentum.ToData());
        }
    }
}