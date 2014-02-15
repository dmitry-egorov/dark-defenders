using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Model.Entities.RigidBodies.Events
{
    internal class AcceleratedAndMoved : EventOf<RigidBody>
    {
        private readonly Vector _newPosition;
        private readonly Momentum _newMomentum;

        public AcceleratedAndMoved(RigidBody rigidBody, Momentum newMomentum, Vector newPosition) 
            : base(rigidBody)
        {
            _newMomentum = newMomentum;
            _newPosition = newPosition;
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<RigidBody> id)
        {
            reciever.Moved(id, _newPosition);
        }

        protected override void Apply(RigidBody rigidBody)
        {
            rigidBody.Accelerated(_newMomentum);
            rigidBody.Moved(_newPosition);
        }
    }
}