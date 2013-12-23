using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Worlds.Events
{
    public class WorldCreated : EventBase<WorldId, WorldCreated>, IDomainEvent
    {
        public Vector SpawnPosition { get; private set; }

        public WorldCreated(WorldId worldId, Vector spawnPosition)
            : base(worldId)
        {
            SpawnPosition = spawnPosition;
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

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}