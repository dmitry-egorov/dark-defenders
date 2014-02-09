using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class Accelerated : EventOf<RigidBody>
    {
        private readonly Momentum _newMomentum;

        public Accelerated(RigidBody rigidBody, Momentum newMomentum) : base(rigidBody)
        {
            _newMomentum = newMomentum;
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<RigidBody> id)
        {
        }

        protected override void Apply(RigidBody rigidBody)
        {
            rigidBody.Accelerated(_newMomentum);
        }
    }
}