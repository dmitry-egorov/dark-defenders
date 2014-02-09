using DarkDefenders.Domain.Data.Entities.RigidBodies;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class ExternalForceChanged : Event<RigidBody>
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

        protected override object CreateData(IdentityOf<RigidBody> id)
        {
            return new ExternalForceChangedData(id, _externalForce);
        }
    }
}
