using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Events;

namespace DarkDefenders.Remote.Model.Internals.Entities
{
    internal class RemotePlayer : IPlayerEvents
    {
        private readonly RemoteEventAdapter _adapter;

        public RemotePlayer(RemoteEventAdapter adapter)
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