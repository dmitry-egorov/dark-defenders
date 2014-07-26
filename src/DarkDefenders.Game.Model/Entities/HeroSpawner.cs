using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DarkDefenders.Game.Model.EntityProperties;
using DarkDefenders.Game.Model.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;
using JetBrains.Annotations;

namespace DarkDefenders.Game.Model.Entities
{
    [UsedImplicitly]
    public class HeroSpawner : Entity<HeroSpawner, IHeroSpawnerEvents>, IHeroSpawnerEvents
    {
        private readonly Random _random;
        private readonly IResources<WorldProperties> _resources;
        private readonly IFactory<HeroSpawnPoint> _heroSpawnPointFactory;

        private IReadOnlyList<HeroSpawnPoint> _spawnPoints;

        public HeroSpawner(IFactory<HeroSpawnPoint> heroSpawnPointFactory, Random random, IResources<WorldProperties> resources)
        {
            _random = random;
            _resources = resources;
            _heroSpawnPointFactory = heroSpawnPointFactory;
        }

        public void Create(string mapId)
        {
            var positions = _resources[mapId].HeroesSpawnPositions;

            var heroSpawnPoints = new List<HeroSpawnPoint>();
            foreach (var position in positions)
            {
                var spawnPoint = _heroSpawnPointFactory.Create();

                spawnPoint.Create(position);

                heroSpawnPoints.Add(spawnPoint);
            }

            CreationEvent(x => x.Created(heroSpawnPoints.AsReadOnly()));
        }

        public void Update()
        {
            _spawnPoints.ForAll(x => x.Update());
        }

        public void ChangeSpawnHeroes(bool enabled)
        {
            _spawnPoints.ForAll(x => x.ChangeSpawnHeroes(enabled));
        }

        public void SpawnHeroes(int count)
        {
            var spawner = _random.ElementFrom(_spawnPoints);
            
            spawner.SpawnHeroes(count);
        }

        void IHeroSpawnerEvents.Created(ReadOnlyCollection<HeroSpawnPoint> spawnPoints)
        {
            _spawnPoints = spawnPoints;
        }
    }
}