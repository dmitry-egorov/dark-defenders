using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Events;

namespace DarkDefenders.Remote.Model.Internals.Entities
{
    internal class RemoteProjectile: IProjectileEvents
    {
        private readonly RemoteEventAdapter _adapter;

        public RemoteProjectile(RemoteEventAdapter adapter)
        {
            _adapter = adapter;
        }

        public void Created(RigidBody rigidBodyId)
        {
            _adapter.ProjectileCreated(rigidBodyId);
        }

        public void Destroyed()
        {
            
        }
    }
}