using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Terrains.Events
{
    public class TerrainCreated : EventBase<TerrainId, TerrainCreated>, ITerrainEvent
    {
        public Vector SpawnPosition { get; private set; }

        public TerrainCreated(TerrainId playerId, Vector spawnPosition)
            : base(playerId)
        {
            SpawnPosition = spawnPosition;
        }

        public void ApplyTo(ITerrainEventsReciever reciever)
        {
            reciever.Apply(this);
        }

        protected override string EventToString()
        {
            return "Terrain created: {0}, {1}".FormatWith(RootId, SpawnPosition);
        }

        protected override bool EventEquals(TerrainCreated other)
        {
            return SpawnPosition == other.SpawnPosition;
        }

        protected override int GetEventHashCode()
        {
            return SpawnPosition.GetHashCode();
        }
    }
}