using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Remote.Model.Internals.Entities
{
    internal class RemotePlayer: IPlayerEvents
    {
        private readonly RemoteEventAdapter _adapter;

        public RemotePlayer(RemoteEventAdapter adapter)
        {
            _adapter = adapter;
        }

        public void Created(IdentityOf<Creature> creatureId)
        {
            _adapter.PlayerCreated(creatureId);
        }

        public void Destroyed()
        {
            
        }
    }
}