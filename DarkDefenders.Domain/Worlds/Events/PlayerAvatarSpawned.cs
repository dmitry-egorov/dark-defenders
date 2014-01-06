using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

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

        protected override string ToStringInternal()
        {
            return "Player's avatar spawned: {0}, {1}".FormatWith(RootId, CreatureId);
        }

        protected override bool EventEquals(PlayerAvatarSpawned other)
        {
            return CreatureId.Equals(other.CreatureId);
        }

        protected override int GetEventHashCode()
        {
            return CreatureId.GetHashCode();
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }

        public void ApplyTo(IWorldEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}