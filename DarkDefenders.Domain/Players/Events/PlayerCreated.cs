using DarkDefenders.Domain.Terrains;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Players.Events
{
    public class PlayerCreated : EventBase<PlayerId, PlayerCreated>, IPlayerEvent
    {
        public TerrainId TerrainId { get; private set; }

        public Vector SpawnPosition { get; private set; }

        public PlayerCreated(PlayerId playerId, TerrainId terrainId, Vector spawnPosition) : base(playerId)
        {
            TerrainId = terrainId;

            SpawnPosition = spawnPosition;
        }

        public void ApplyTo(IPlayerEventsReciever reciever)
        {
            reciever.Apply(this);
        }

        protected override string EventToString()
        {
            return "Player created {0}, {1}, {2}".FormatWith(RootId, TerrainId, SpawnPosition);
        }

        protected override bool EventEquals(PlayerCreated other)
        {
            return TerrainId.Equals(other.TerrainId) && SpawnPosition.Equals(other.SpawnPosition);
        }

        protected override int GetEventHashCode()
        {
            return (TerrainId.GetHashCode() * 397) ^ SpawnPosition.GetHashCode();
        }
    }
}