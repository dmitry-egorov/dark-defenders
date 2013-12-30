using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Worlds.Events
{
    public class WorldCreated : EventBase<WorldId, WorldCreated>, IDomainEvent
    {
        public Dimensions Dimensions { get; private set; }
        public Vector SpawnPosition { get; private set; }

        public WorldCreated(WorldId worldId, Dimensions dimensions, Vector spawnPosition)
            : base(worldId)
        {
            Dimensions = dimensions;
            SpawnPosition = spawnPosition;
        }

        protected override string ToStringInternal()
        {
            return "World created: {0}, {1}".FormatWith(RootId, SpawnPosition);
        }

        protected override bool EventEquals(WorldCreated other)
        {
            return Dimensions.Equals(other.Dimensions) && SpawnPosition.Equals(other.SpawnPosition);
        }

        protected override int GetEventHashCode()
        {
            return (Dimensions.GetHashCode() * 397) ^ SpawnPosition.GetHashCode();
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}