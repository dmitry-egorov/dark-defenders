using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Remote.Model.Internals.Entities
{
    internal class RemoteRigidBody: IRigidBodyEvents
    {
        private readonly RemoteEventAdapter _adapter;
        private IdentityOf<RigidBody> _rigidBodyId;

        public RemoteRigidBody(RemoteEventAdapter adapter)
        {
            _adapter = adapter;
        }

        public void Created(IdentityOf<RigidBody> rigidBodyId, Vector initialPosition, Momentum initialMomentum, string properties)
        {
            _adapter.RigidBodyCreated(rigidBodyId, initialPosition);
            _rigidBodyId = rigidBodyId;
        }

        public void Accelerated(Momentum newMomentum)
        {
        }

        public void Moved(Vector newPosition)
        {
            _adapter.Moved(_rigidBodyId, newPosition);
        }

        public void ExternalForceChanged(Force externalForce)
        {
        }

        public void Destroyed()
        {
            _adapter.RigidBodyDestroyed(_rigidBodyId);
        }
    }
}