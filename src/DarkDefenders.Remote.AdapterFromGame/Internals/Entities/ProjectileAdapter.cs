using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Game.Model.Events;

namespace DarkDefenders.Remote.AdapterFromGame.Internals.Entities
{
    internal class ProjectileAdapter : IProjectileEvents
    {
        private readonly RemoteEventsAdapter _adapter;

        public ProjectileAdapter(RemoteEventsAdapter adapter)
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