using System.Collections.Generic;
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

        public IEnumerable<IEvent> Create(WorldId worldId, Vector spawnPosition)
        {
            AssertDoesntExist(worldId);
            return new WorldCreated(worldId, spawnPosition).EnumerateOnce();
        }

        protected override World Handle(WorldCreated creationEvent)
        {
            return new World(creationEvent.RootId, creationEvent.SpawnPosition);
        }
    }
}