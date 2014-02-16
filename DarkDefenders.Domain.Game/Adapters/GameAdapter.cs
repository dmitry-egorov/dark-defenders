using System;
using DarkDefenders.Domain.Game.Interfaces;
using DarkDefenders.Domain.Model.Entities;
using Infrastructure.DDDES;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Game.Adapters
{
    [UsedImplicitly]
    internal class GameAdapter : IGame
    {
        private readonly IEventsProcessor _processor;

        private readonly World _world;
        private readonly HeroSpawner _heroSpawner;
        private readonly PlayerSpawner _playerSpawner;

        public GameAdapter
        (
            IEventsProcessor processor,
            World world,
            PlayerSpawner playerSpawner,
            HeroSpawner heroSpawner
        )
        {
            _processor = processor;

            _world = world;
            _playerSpawner = playerSpawner;
            _heroSpawner = heroSpawner;
        }

        public void Initialize(string mapId)
        {
            _world.Create(mapId);

            _processor.Process();
        }

        public void Update(TimeSpan elapsed)
        {
            _world.Update(elapsed);

            _processor.Process();
        }

        public void KillAllHeroes()
        {
            _world.KillAllHeroes();
        }

        public IPlayer AddPlayer()
        {
            var avatar = _playerSpawner.Spawn();

            return new PlayerAdapter(avatar);
        }

        public void SpawnHeros(int count)
        {
            _heroSpawner.SpawnHeroes(count);
        }

        public void ChangeSpawnHeroes(bool enabled)
        {
            _heroSpawner.ChangeSpawnHeroes(enabled);
        }
    }
}