using System;
using DarkDefenders.Domain.Model;
using DarkDefenders.Domain.Model.Entities.HeroSpawners;
using DarkDefenders.Domain.Model.Entities.Players;
using DarkDefenders.Domain.Model.Entities.PlayerSpawners;
using DarkDefenders.Domain.Model.Entities.Worlds;
using DarkDefenders.Game.Interfaces;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations;
using Infrastructure.DDDES.Implementations.Domain;
using JetBrains.Annotations;

namespace DarkDefenders.Game.Adapters
{
    [UsedImplicitly]
    internal class GameAdapter : IGame
    {
        private readonly EventsProcessor<IEventsReciever> _processor;

        private readonly IFactory<Player> _playerFactory;

        private readonly World _world;
        private readonly HeroSpawner _heroSpawner;
        private readonly PlayerSpawner _playerSpawner;

        public GameAdapter
        (
            EventsProcessor<IEventsReciever> processor, 
            IFactory<Player> playerFactory,
            World world,
            PlayerSpawner playerSpawner,
            HeroSpawner heroSpawner
        )
        {
            _processor = processor;

            _playerFactory = playerFactory;
            _world = world;
            _playerSpawner = playerSpawner;
            _heroSpawner = heroSpawner;
        }

        public void Initialize(string mapId)
        {
            var wevents = _world.Create(mapId);
            _processor.Process(wevents);

            _processor.Broadcast();
        }

        public void Update(TimeSpan elapsed)
        {
            var events = _world.Update(elapsed);

            _processor.Process(events);
            _processor.Broadcast();
        }

        public void KillAllHeroes()
        {
            var events = _world.KillAllHeroes();

            _processor.Process(events);
        }

        public IPlayer AddPlayer()
        {
            var avatar = _playerFactory.Create();

            var events = _playerSpawner.Spawn(avatar);

            _processor.Process(events);

            return new PlayerAdapter(new EntityAdapter<Player>(avatar, _processor));
        }

        public void SpawnHero()
        {
            var events = _heroSpawner.Spawn();

            _processor.Process(events);
        }

        public void ChangeSpawnHeroes(bool enabled)
        {
            var events = _heroSpawner.ChangeSpawnHeroes(enabled);

            _processor.Process(events);
        }
    }
}