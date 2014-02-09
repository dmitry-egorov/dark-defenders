using DarkDefenders.Domain.Infrastructure;

using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class Moved : EventOf<RigidBody>
    {
        private readonly Vector _newPosition;

        public Moved(RigidBody rigidBody, Vector newPosition) : base(rigidBody)
        {
            _newPosition = newPosition;
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<RigidBody> id)
        {
            reciever.Moved(id, _newPosition);
        }

        protected override void Apply(RigidBody rigidBody)
        {
            rigidBody.SetNewPosition(_newPosition);
        }
    }
}