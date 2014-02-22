using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Game.Model.Events;

namespace DarkDefenders.Remote.AdapterFromGame.Internals.Entities
{
    internal class PlayerAdapter : IPlayerEvents
    {
        private readonly RemoteEventAdapter _adapter;

        public PlayerAdapter(RemoteEventAdapter adapter)
        {
            _adapter = adapter;
        }

        public void Created(Creature creature)
        {
            _adapter.PlayerCreated(creature);
        }

        public void Destroyed()
        {
            
        }
    }
}