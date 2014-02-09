using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class ExternalForceChanged : EventOf<RigidBody>
    {
        private readonly Force _externalForce;

        public ExternalForceChanged(RigidBody rigidBody, Force externalForce) : base(rigidBody)
        {
            _externalForce = externalForce;
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<RigidBody> id)
        {
        }

        protected override void Apply(RigidBody rigidBody)
        {
            rigidBody.SetExternalForce(_externalForce);
        }
    }
}
