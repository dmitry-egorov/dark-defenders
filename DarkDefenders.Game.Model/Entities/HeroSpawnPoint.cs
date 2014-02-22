using System;
using DarkDefenders.Game.Model.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using JetBrains.Annotations;

namespace DarkDefenders.Game.Model.Entities
{
    [UsedImplicitly]
    public class HeroSpawnPoint : Entity<HeroSpawnPoint, IHeroSpawnPointEvents>, IHeroSpawnPointEvents
    {
        private readonly TimeSpan _heroesSpawnCooldownTime = TimeSpan.FromSeconds(10);
        private readonly IFactory<Hero> _heroFactory;
        private readonly Cooldown _heroSpawnCooldown;

        private Vector _position;
        private bool _enabled;
        private int _queuedForSpawnCount;

        public HeroSpawnPoint(IFactory<Hero> heroFactory, Cooldown heroSpawnCooldown)
        {
            _heroFactory = heroFactory;
            _heroSpawnCooldown = heroSpawnCooldown;
        }

        public void Create(Vector position)
        {
            _heroSpawnCooldown.Create(_heroesSpawnCooldownTime);
            CreationEvent(x => x.Created(position));
        }

        public void Update()
        {
            if (_queuedForSpawnCount > 0)
            {
                Spawn();
                
                DecrementQueuedCount();

                return;
            }

            if (!_enabled)
            {
                return;
            }

            _heroSpawnCooldown.Activate(Spawn);
        }

        public void SpawnHeroes(int count)
        {
            var newQueuedForSpawnCount = _queuedForSpawnCount + count;

            Event(x => x.QueuedForSpawnCountChanged(newQueuedForSpawnCount));
        }
        
        public void ChangeSpawnHeroes(bool enabled)
        {
            Event(x => x.SpawnHeroesChanged(enabled));
        }

        void IHeroSpawnPointEvents.Created(Vector position)
        {
            _position = position;
            _enabled = true;
        }

        void IHeroSpawnPointEvents.SpawnHeroesChanged(bool enabled)
        {
            _enabled = enabled;
        }

        void IHeroSpawnPointEvents.QueuedForSpawnCountChanged(int newQueuedForSpawnCount)
        {
            _queuedForSpawnCount = newQueuedForSpawnCount;
        }

        private void Spawn()
        {
            _heroFactory.Create().Create(_position);
        }

        private void DecrementQueuedCount()
        {
            var newQueuedForSpawnCount = _queuedForSpawnCount - 1;
            Event(x => x.QueuedForSpawnCountChanged(newQueuedForSpawnCount));
        }
    }
}