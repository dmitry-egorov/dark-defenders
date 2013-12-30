using System.Collections.Generic;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Worlds
{
    public class WorldFactory : RootFactory<WorldId, World, WorldCreated>
    {
        public WorldFactory(IRepository<WorldId, World> repository) : base(repository)
        {
        }

        public IEnumerable<IDomainEvent> Create(WorldId worldId, Dimensions dimensions, Vector spawnPosition)
        {
            AssertDoesntExist(worldId);
            return new WorldCreated(worldId, dimensions, spawnPosition).EnumerateOnce();
        }

        protected override World Handle(WorldCreated creationEvent)
        {
            return new World(creationEvent.RootId, creationEvent.Dimensions, creationEvent.SpawnPosition);
        }
    }
}