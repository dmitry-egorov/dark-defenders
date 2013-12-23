using System.Collections.Generic;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Worlds
{
    public class WorldFactory : Factory<WorldId, World>, IFactory<World, WorldCreated>
    {
        public WorldFactory(IRepository<WorldId, World> repository) : base(repository)
        {
        }

        World IFactory<World, WorldCreated>.Handle(WorldCreated creationEvent)
        {
            return new World(creationEvent.RootId, creationEvent.SpawnPosition);
        }

        public IEnumerable<IEvent> Create(WorldId worldId, Vector spawnPosition)
        {
            AssertDoesntExist(worldId);
            return new WorldCreated(worldId, spawnPosition).EnumerateOnce();
        }
    }
}