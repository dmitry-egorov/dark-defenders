using System;
using DarkDefenders.Game.App.Interfaces;
using DarkDefenders.Game.Model.Entities;
using Infrastructure.DDDES;
using JetBrains.Annotations;

namespace DarkDefenders.Game.App.Internals
{
    [UsedImplicitly]
    internal class GameService : IGameService
    {
        private readonly IEventsProcessor _processor;

        private readonly World _world;
        private readonly HeroSpawner _heroSpawner;
        private readonly PlayerSpawner _playerSpawner;

        public GameService
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
            _processor.Process();

            _world.Update(elapsed);

            _processor.Process();
        }

        public void KillAllHeroes()
        {
            _world.KillAllHeroes();
        }

        public IPlayerService AddPlayer()
        {
            var avatar = _playerSpawner.Spawn();

            return new PlayerService(avatar);
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