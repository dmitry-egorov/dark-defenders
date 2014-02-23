using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Game.Model.Events;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Remote.AdapterFromGame.Internals.Entities
{
    internal class RigidBodyAdapter : IRigidBodyEvents
    {
        private readonly RemoteEventsAdapter _adapter;

        private RigidBody _rigidBody;

        public RigidBodyAdapter(RemoteEventsAdapter adapter)
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