using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Players.Events
{
    public class PlayerCreated : EventBase<PlayerId, PlayerCreated>, IPlayerEvent
    {
        public WorldId WorldId { get; private set; }

        public Vector SpawnPosition { get; private set; }

        public PlayerCreated(PlayerId playerId, WorldId worldId, Vector spawnPosition) : base(playerId)
        {
            WorldId = worldId;

            SpawnPosition = spawnPosition;
        }

        public void ApplyTo(IPlayerEventsReciever reciever)
        {
            reciever.Apply(this);
        }

        protected override string EventToString()
        {
            return "Player created {0}, {1}, {2}".FormatWith(RootId, WorldId, SpawnPosition);
        }

        protected override bool EventEquals(PlayerCreated other)
        {
            return WorldId.Equals(other.WorldId) && SpawnPosition.Equals(other.SpawnPosition);
        }

        protected override int GetEventHashCode()
        {
            return (WorldId.GetHashCode() * 397) ^ SpawnPosition.GetHashCode();
        }
    }
}