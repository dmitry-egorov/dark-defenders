using System;
using System.Collections.Generic;
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

        private readonly TimeSpan _heroesSpawnCooldown = TimeSpan.FromSeconds(10);
        
        private Cooldown _heroSpawnCooldown;
        private Vector _position;
        private bool _enabled;
        private readonly IFactory<Hero> _heroFactory;
        private readonly Clock _clock;

        public HeroSpawnPoint(IHeroSpawnPointEvents external, IStorage<HeroSpawnPoint> storage, IFactory<Hero> heroFactory, CooldownFactory cooldownFactory, Clock clock) 
            : base(external, storage)
        {
            _cooldownFactory = cooldownFactory;
            _heroFactory = heroFactory;
            _clock = clock;
        }

        public IEnumerable<IEvent> Create(Vector position)
        {
            yield return CreationEvent(x => x.Created(position));
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

            yield return Event(x => x.ActivationTimeChanged(currentTime));
        }

        public IEnumerable<IEvent> ChangeSpawnHeroes(bool enabled)
        {
            yield return Event(x => x.SpawnHeroesChanged(enabled));
        }

        void IHeroSpawnPointEvents.Created(Vector position)
        {
            _position = position;
            _enabled = true;
            _heroSpawnCooldown = _cooldownFactory.Create(_heroesSpawnCooldown);
        }

        void IHeroSpawnPointEvents.ActivationTimeChanged(TimeSpan time)
        {
            _heroSpawnCooldown.SetLastActivationTime(time);
        }

        void IHeroSpawnPointEvents.SpawnHeroesChanged(bool enabled)
        {
            _enabled = enabled;
        }

        void  IEntityEvents.Destroyed()
        {
        }
    }
}