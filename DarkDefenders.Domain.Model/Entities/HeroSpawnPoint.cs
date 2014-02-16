using System;
using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities
{
    [UsedImplicitly]
    public class HeroSpawnPoint : Entity<HeroSpawnPoint, IHeroSpawnPointEvents>, IHeroSpawnPointEvents
    {
        private readonly CooldownFactory _cooldownFactory;

        private readonly TimeSpan _heroesSpawnCooldownTime = TimeSpan.FromSeconds(10);
        private readonly IFactory<Hero> _heroFactory;
        private readonly Clock _clock;

        private Cooldown _heroSpawnCooldown;
        private Vector _position;
        private bool _enabled;
        private int _queuedForSpawnCount;

        public HeroSpawnPoint(IFactory<Hero> heroFactory, CooldownFactory cooldownFactory, Clock clock)
        {
            _cooldownFactory = cooldownFactory;
            _heroFactory = heroFactory;
            _clock = clock;
        }

        public void Create(Vector position)
        {
            CreationEvent(x => x.Created(position));
        }

        public void Update()
        {
            if (_queuedForSpawnCount > 0)
            {
                var newQueuedForSpawnCount = _queuedForSpawnCount - 1;

                Event(x => x.QueuedForSpawnCountChanged(newQueuedForSpawnCount));
            }
            else if (!_enabled || _heroSpawnCooldown.IsInEffect())
            {
                return;
            }

            Spawn();
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
            _heroSpawnCooldown = _cooldownFactory.Create(_heroesSpawnCooldownTime);
        }

        void IHeroSpawnPointEvents.ActivationTimeChanged(TimeSpan time)
        {
            _heroSpawnCooldown.SetLastActivationTime(time);
        }

        void IHeroSpawnPointEvents.SpawnHeroesChanged(bool enabled)
        {
            _enabled = enabled;
        }

        public void QueuedForSpawnCountChanged(int newQueuedForSpawnCount)
        {
            _queuedForSpawnCount = newQueuedForSpawnCount;
        }

        private void Spawn()
        {
            var hero = _heroFactory.Create();
            hero.Create(_position);

            var currentTime = _clock.GetCurrentTime();

            Event(x => x.ActivationTimeChanged(currentTime));
        }
    }
}