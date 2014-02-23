using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Game.Model.Events;

namespace DarkDefenders.Remote.AdapterFromGame.Internals.Entities
{
    internal class PlayerAdapter : IPlayerEvents
    {
        private readonly RemoteEventsAdapter _adapter;

        public PlayerAdapter(RemoteEventsAdapter adapter)
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