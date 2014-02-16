using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Remote.Model.Internals.Entities
{
    internal class RemoteProjectile: IProjectileEvents
    {
        private readonly RemoteEventAdapter _adapter;

        public RemoteProjectile(RemoteEventAdapter adapter)
        {
            _adapter = adapter;
        }

        public void Created(IdentityOf<RigidBody> rigidBodyId)
        {
            _adapter.ProjectileCreated(rigidBodyId);
        }

        public void Destroyed()
        {
            
        }
    }
}