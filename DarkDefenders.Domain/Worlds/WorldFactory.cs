using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Heroes;
using DarkDefenders.Domain.Other;
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

        public WorldFactory(IRepository<WorldId, World> repository, IRepository<ClockId, Clock> clockRepository, CreatureFactory creatureFactory, HeroFactory heroFactory) : base(repository)
        {
            _creatureFactory = creatureFactory;
            _heroFactory = heroFactory;
            _clockRepository = clockRepository;
        }

        public IEnumerable<IDomainEvent> Create(WorldId worldId, ClockId clockId, Map<Tile> map, Vector spawnPosition, CreatureProperties playersAvatarProperties, Vector heroesSpawnPosition, TimeSpan heroesSpawnCooldown, CreatureProperties heroesCreatureProperties)
        {
            AssertDoesntExist(worldId);

            return new WorldCreated(worldId, clockId, map, spawnPosition, playersAvatarProperties, heroesSpawnPosition, heroesSpawnCooldown, heroesCreatureProperties).EnumerateOnce();
        }

        protected override World Handle(WorldCreated creationEvent)
        {
            var worldId = creationEvent.RootId;
            var terrain = creationEvent.Map;
            var spawnPosition = creationEvent.PlayersSpawnPosition;
            var playersAvatarProperties = creationEvent.PlayersAvatarProperties;
            var heroesSpawnPosition = creationEvent.HeroesSpawnPosition;
            var heroesSpawnCooldown = creationEvent.HeroesSpawnCooldown;
            var heroesCreatureProperties = creationEvent.HeroesCreatureProperties;

            var clock = _clockRepository.GetById(creationEvent.ClockId);

            return new World(worldId, clock, _creatureFactory, terrain, spawnPosition, playersAvatarProperties, heroesSpawnPosition, heroesSpawnCooldown, heroesCreatureProperties, _heroFactory);
        }
    }
}