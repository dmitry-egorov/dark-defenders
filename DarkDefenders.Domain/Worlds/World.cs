using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Heroes;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Terrains;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Worlds
{
    public class World : RootBase<WorldId, IWorldEventsReciever, IWorldEvent>, IWorldEventsReciever
    {
        public IEnumerable<IDomainEvent> SpawnPlayerAvatar(CreatureId creatureId)
        {
            var rigidBodyId = new RigidBodyId();

            var position = _random.From(_playersSpawnPositions);

            var events = _creatureFactory.Create(creatureId, rigidBodyId, _clock.Id, _terrainId, position, _playerAvatarProperties);

            foreach (var e in events) { yield return e; }

            yield return new PlayerAvatarSpawned(Id, creatureId);
        }

        public IEnumerable<IDomainEvent> SpawnHeroes()
        {
            if (_heroSpawnCooldown.IsInEffect())
            {
                yield break;
            }

            var heroId = new HeroId();

            var events = SpawnHero(heroId);

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IDomainEvent> SpawnHero()
        {
            var heroId = new HeroId();
            return SpawnHero(heroId);
        }

        public IEnumerable<IDomainEvent> KillAllHeroes()
        {
            var heroes = _heroRepository.GetAll().AsReadOnly();

            foreach (var hero in heroes)
            {
                var events = hero.Kill();
                foreach (var e in events) { yield return e; }
            }
        }

        public void Recieve(HeroSpawned heroSpawned)
        {
            _heroSpawnCooldown.SetLastActivationTime(heroSpawned.Time);
        }

        public void Recieve(PlayerAvatarSpawned playerAvatarSpawned)
        {
            
        }

        internal World
            (
                WorldId id, 
                Clock clock, 
                CreatureFactory creatureFactory, 
                IEnumerable<Vector> playersSpawnPositions, 
                CreatureProperties playerAvatarProperties, 
                IEnumerable<Vector> heroesSpawnPositions, 
                TimeSpan heroesSpawnCooldown, 
                CreatureProperties heroesCreatureProperties, 
                HeroFactory heroFactory, 
                TerrainId terrainId, Random random, IRepository<HeroId, Hero> heroRepository) : base(id)
        {
            _creatureFactory = creatureFactory;
            _heroesSpawnPositions = heroesSpawnPositions.AsReadOnly();
            _heroesCreatureProperties = heroesCreatureProperties;
            _heroFactory = heroFactory;
            _terrainId = terrainId;
            _random = random;
            _heroRepository = heroRepository;
            _playerAvatarProperties = playerAvatarProperties;

            _playersSpawnPositions = playersSpawnPositions.AsReadOnly();

            _clock = clock;
            _heroSpawnCooldown = new Cooldown(clock, heroesSpawnCooldown);
        }

        private IEnumerable<IDomainEvent> SpawnHero(HeroId heroId)
        {
            var position = _random.From(_heroesSpawnPositions);

            var events = _heroFactory.Create(heroId, _clock.Id, _terrainId, position, _heroesCreatureProperties);

            foreach (var e in events) { yield return e; }

            yield return new HeroSpawned(Id, _clock.GetCurrentTime(), heroId);
        }


        private readonly CreatureFactory _creatureFactory;
        private readonly IRepository<HeroId, Hero> _heroRepository;

        private readonly ReadOnlyCollection<Vector> _playersSpawnPositions;
        private readonly CreatureProperties _playerAvatarProperties;
        private readonly Clock _clock;
        private readonly Cooldown _heroSpawnCooldown;
        private readonly ReadOnlyCollection<Vector> _heroesSpawnPositions;
        private readonly CreatureProperties _heroesCreatureProperties;
        private readonly HeroFactory _heroFactory;
        private readonly TerrainId _terrainId;
        private readonly Random _random;
    }
}
