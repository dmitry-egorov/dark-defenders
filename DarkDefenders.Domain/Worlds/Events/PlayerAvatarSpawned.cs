using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Worlds.Events
{
    public class PlayerAvatarSpawned : EventBase<WorldId, PlayerAvatarSpawned>, IWorldEvent
    {
        public CreatureId CreatureId { get; private set; }

        public PlayerAvatarSpawned(WorldId worldId, CreatureId creatureId)
            : base(worldId)
        {
            CreatureId = creatureId;
        }

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }

        public void ApplyTo(IWorldEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}