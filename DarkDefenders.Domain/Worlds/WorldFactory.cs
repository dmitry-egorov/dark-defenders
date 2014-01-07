using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Heroes;
using DarkDefenders.Domain.Terrains;
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
        private readonly IRepository<ClockId, Clock> _clockRepository;
        private readonly HeroFactory _heroFactory;
        private readonly Random _random;
        private readonly IRepository<HeroId, Hero> _heroRepository;

        public WorldFactory(IRepository<WorldId, World> repository, IRepository<ClockId, Clock> clockRepository, IRepository<HeroId, Hero> heroRepository, CreatureFactory creatureFactory, HeroFactory heroFactory, Random random) : base(repository)
        {
            _creatureFactory = creatureFactory;
            _heroFactory = heroFactory;
            _random = random;
            _heroRepository = heroRepository;
            _clockRepository = clockRepository;
        }

        public IEnumerable<IDomainEvent> Create(WorldId worldId, ClockId clockId, TerrainId terrainId, IEnumerable<Vector> spawnPositions, CreatureProperties playersAvatarProperties, IEnumerable<Vector> heroesSpawnPositions, TimeSpan heroesSpawnCooldown, CreatureProperties heroesCreatureProperties)
        {
            AssertDoesntExist(worldId);

            return new WorldCreated(worldId, clockId, terrainId, spawnPositions, playersAvatarProperties, heroesSpawnPositions, heroesSpawnCooldown, heroesCreatureProperties).EnumerateOnce();
        }

        protected override World Handle(WorldCreated creationEvent)
        {
            var worldId = creationEvent.RootId;
            var spawnPosition = creationEvent.PlayersSpawnPosition;
            var playersAvatarProperties = creationEvent.PlayersAvatarProperties;
            var heroesSpawnPosition = creationEvent.HeroesSpawnPositions;
            var heroesSpawnCooldown = creationEvent.HeroesSpawnCooldown;
            var heroesCreatureProperties = creationEvent.HeroesCreatureProperties;

            var clock = _clockRepository.GetById(creationEvent.ClockId);

            return new World(worldId, clock, _creatureFactory, spawnPosition, playersAvatarProperties, heroesSpawnPosition, heroesSpawnCooldown, heroesCreatureProperties, _heroFactory, creationEvent.TerrainId, _random, _heroRepository);
        }
    }
}