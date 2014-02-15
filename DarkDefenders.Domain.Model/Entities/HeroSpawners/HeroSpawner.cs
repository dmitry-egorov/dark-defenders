using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DarkDefenders.Domain.Model.Entities.HeroSpawners.Events;
using DarkDefenders.Domain.Model.Entities.HeroSpawnPoints;
using DarkDefenders.Domain.Model.Entities.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities.HeroSpawners
{
    [UsedImplicitly]
    public class HeroSpawner : Entity<HeroSpawner>
    {
        private readonly IStorage<HeroSpawner> _storage;
        private readonly Random _random;
        private readonly IResources<WorldProperties> _resources;
        private readonly IFactory<HeroSpawnPoint> _heroSpawnPointFactory;

        private IReadOnlyList<HeroSpawnPoint> _spawnPoints;

        public HeroSpawner(IStorage<HeroSpawner> storage, IFactory<HeroSpawnPoint> heroSpawnPointFactory, Random random, IResources<WorldProperties> resources)
        {
            _storage = storage;
            _random = random;
            _resources = resources;
            _heroSpawnPointFactory = heroSpawnPointFactory;
        }

        public IEnumerable<IEvent> Create(string mapId)
        {
            var positions = _resources[mapId].HeroesSpawnPositions;

            var heroSpawnPoints = new List<HeroSpawnPoint>();
            var hspevents = Enumerable.Empty<IEvent>();
            foreach (var position in positions)
            {
                var spawnPoint = _heroSpawnPointFactory.Create();
                var sevents = spawnPoint.Create(position);

                hspevents = hspevents.Concat(sevents);

                heroSpawnPoints.Add(spawnPoint);
            }

            foreach (var e in hspevents) { yield return e; }

            yield return new HeroSpawnerCreated(this, _storage, heroSpawnPoints.AsReadOnly());
        }

        public IEnumerable<IEvent> Update()
        {
            var spawnPoint = _random.ElementFrom(_spawnPoints);

            var events = spawnPoint.Update();

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IEvent> ChangeSpawnHeroes(bool enabled)
        {
            var events = _spawnPoints.ForAll(x => x.ChangeSpawnHeroes(enabled));
            
            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IEvent> Spawn()
        {
            var spawner = _random.ElementFrom(_spawnPoints);
            
            var events = spawner.ForceSpawn();
            
            foreach (var e in events) { yield return e; }
        }

        internal void Created(ReadOnlyCollection<HeroSpawnPoint> spawnPoints)
        {
            _spawnPoints = spawnPoints;
        }
    }
}