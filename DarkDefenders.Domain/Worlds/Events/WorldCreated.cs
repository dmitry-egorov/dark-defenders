using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Worlds.Events
{
    public class WorldCreated : EventBase<WorldId, WorldCreated>, IWorldEvent
    {
        public Vector SpawnPosition { get; private set; }

        public WorldCreated(WorldId playerId, Vector spawnPosition)
            : base(playerId)
        {
            SpawnPosition = spawnPosition;
        }

        public void ApplyTo(IWorldEventsReciever reciever)
        {
            reciever.Apply(this);
        }

        protected override string ToStringInternal()
        {
            return "World created: {0}, {1}".FormatWith(RootId, SpawnPosition);
        }

        protected override bool EventEquals(WorldCreated other)
        {
            return SpawnPosition.Equals(other.SpawnPosition);
        }

        protected override int GetEventHashCode()
        {
            return SpawnPosition.GetHashCode();
        }
    }
}