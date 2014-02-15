using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Model.Entities.Clocks;
using DarkDefenders.Domain.Model.Entities.Heroes;
using DarkDefenders.Domain.Model.Entities.HeroSpawnPoints.Events;
using DarkDefenders.Domain.Model.Entities.Worlds.Events;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities.HeroSpawnPoints
{
    [UsedImplicitly]
    public class HeroSpawnPoint : Entity<HeroSpawnPoint>
    {
        private readonly IStorage<HeroSpawnPoint> _storage;
        private readonly CooldownFactory _cooldownFactory;

        private readonly TimeSpan _heroesSpawnCooldown = TimeSpan.FromSeconds(10);
        
        private Cooldown _heroSpawnCooldown;
        private Vector _position;
        private bool _enabled;
        private readonly IFactory<Hero> _heroFactory;
        private readonly Clock _clock;

        public HeroSpawnPoint(IStorage<HeroSpawnPoint> storage, IFactory<Hero> heroFactory, CooldownFactory cooldownFactory, Clock clock)
        {
            _storage = storage;
            _cooldownFactory = cooldownFactory;
            _heroFactory = heroFactory;
            _clock = clock;
        }

        public IEnumerable<IEvent> Create(Vector position)
        {
            yield return new HeroSpawnPointCreated(this, _storage, position);
        }

        public IEnumerable<IEvent> Update()
        {
            if (!_enabled || _heroSpawnCooldown.IsInEffect())
            {
                yield break;
            }

            var events = ForceSpawn();

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IEvent> ForceSpawn()
        {
            var hero = _heroFactory.Create();
            var events = hero.Create(_position);

            var currentTime = _clock.GetCurrentTime();

            foreach (var e in events) { yield return e; }

            yield return new ActivationTimeChanged(this, currentTime);
        }

        public IEnumerable<IEvent> ChangeSpawnHeroes(bool enabled)
        {
            yield return new SpawnHeroesChanged(this, enabled);
        }

        internal void Created(Vector position)
        {
            _position = position;
            _enabled = true;
            _heroSpawnCooldown = _cooldownFactory.Create(_heroesSpawnCooldown);
        }

        internal void ActivationTimeChanged(TimeSpan time)
        {
            _heroSpawnCooldown.SetLastActivationTime(time);
        }

        internal void SpawnHeroesChanged(bool enabled)
        {
            _enabled = enabled;
        }
    }
}