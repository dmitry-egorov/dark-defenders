using System.Collections.Generic;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Worlds
{
    public class WorldFactory : RootFactory<WorldId, World, WorldCreated>
    {
        private readonly CreatureFactory _creatureFactory;
        private readonly RigidBodyProperties _playersRigidBodyProperties;

        public WorldFactory(IRepository<WorldId, World> repository, CreatureFactory creatureFactory, RigidBodyProperties playersRigidBodyProperties) : base(repository)
        {
            _creatureFactory = creatureFactory;
            _playersRigidBodyProperties = playersRigidBodyProperties;
        }

        public IEnumerable<IDomainEvent> Create(WorldId worldId, Map<Tile> map, Vector spawnPosition)
        {
            AssertDoesntExist(worldId);
            return new WorldCreated(worldId, map, spawnPosition).EnumerateOnce();
        }

        protected override World Handle(WorldCreated creationEvent)
        {
            var worldId = creationEvent.RootId;
            var terrain = creationEvent.Map;
            var spawnPosition = creationEvent.PlayersSpawnPosition;

            return new World(worldId, terrain, spawnPosition, _playersRigidBodyProperties, _creatureFactory);
        }
    }
}