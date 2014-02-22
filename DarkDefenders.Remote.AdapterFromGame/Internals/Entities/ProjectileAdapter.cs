using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Game.Model.Events;

namespace DarkDefenders.Remote.AdapterFromGame.Internals.Entities
{
    internal class ProjectileAdapter : IProjectileEvents
    {
        private readonly RemoteEventAdapter _adapter;

        public ProjectileAdapter(RemoteEventAdapter adapter)
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