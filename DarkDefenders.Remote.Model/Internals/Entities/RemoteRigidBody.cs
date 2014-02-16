using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Remote.Model.Internals.Entities
{
    internal class RemoteRigidBody: IRigidBodyEvents
    {
        private readonly RemoteEventAdapter _adapter;

        private RigidBody _rigidBody;

        public RemoteRigidBody(RemoteEventAdapter adapter)
        {
            _adapter = adapter;
        }

        public void Created(RigidBody rigidBody, Vector initialPosition, Momentum initialMomentum, string properties)
        {
            _adapter.RigidBodyCreated(rigidBody, initialPosition);
            _rigidBody = rigidBody;
        }

        public void Accelerated(Momentum newMomentum)
        {
        }

        public void Moved(Vector newPosition)
        {
            _adapter.Moved(_rigidBody, newPosition);
        }

        public void ExternalForceChanged(Force externalForce)
        {
        }

        public void Destroyed()
        {
            _adapter.RigidBodyDestroyed(_rigidBody);
        }
    }
}