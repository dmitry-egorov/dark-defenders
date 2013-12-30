using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Other;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Worlds.Events
{
    public class WorldCreated : EventBase<WorldId, WorldCreated>, IDomainEvent
    {
        public Map<Tile> Map { get; private set; }
        public Vector SpawnPosition { get; private set; }

        public WorldCreated(WorldId worldId, Map<Tile> map, Vector spawnPosition)
            : base(worldId)
        {
            Map = map;
            SpawnPosition = spawnPosition;
        }

        protected override string ToStringInternal()
        {
            return "World created: {0}, {1}".FormatWith(RootId, SpawnPosition);
        }

        protected override bool EventEquals(WorldCreated other)
        {
            return Map.Equals(other.Map)
                && SpawnPosition.Equals(other.SpawnPosition);
        }

        protected override int GetEventHashCode()
        {
            return (Map.GetHashCode() * 397) ^ SpawnPosition.GetHashCode();
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}