using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.Heroes;
using DarkDefenders.Domain.Entities.Other;
using DarkDefenders.Domain.Entities.Worlds.Events;
using DarkDefenders.Domain.Interfaces;
using DarkDefenders.Dtos.Entities.Creatures;
using DarkDefenders.Dtos.Entities.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Entities.Worlds
{
    internal class World : Entity<WorldId>
    {
        private readonly CreatureFactory _creatureFactory;
        private readonly HeroFactory _heroFactory;

        private readonly IContainer<Clock> _clockContainer;
        private readonly Random _random;

        private readonly ReadOnlyCollection<Vector> _playersSpawnPositions;
        private readonly CreatureProperties _playersAvatarProperties;
        private readonly Cooldown _heroSpawnCooldown;
        private readonly ReadOnlyCollection<Vector> _heroesSpawnPositions;
        private readonly CreatureProperties _heroesCreatureProperties;

        private bool _spawnHeroes = true;

        public World
        (
            HeroFactory heroFactory, 
            CreatureFactory creatureFactory, 
            IContainer<Clock> clockContainer, 
            Random random, 
            WorldProperties properties
        )
        {
            _creatureFactory = creatureFactory;
            _heroesSpawnPositions = properties.HeroesSpawnPositions;
            _heroesCreatureProperties = properties.HeroesCreatureProperties;
            _heroFactory = heroFactory;
            _random = random;
            _playersAvatarProperties = properties.PlayersAvatarProperties;

            _playersSpawnPositions = properties.PlayersSpawnPositions;

            _clockContainer = clockContainer;
            _heroSpawnCooldown = new Cooldown(clockContainer, properties.HeroesSpawnCooldown);
        }

        public IEnumerable<IEvent> SpawnHeroes()
        {
            if (!_spawnHeroes || _heroSpawnCooldown.IsInEffect())
            {
                yield break;
            }

            var events = SpawnHero();

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IEvent> ChangeSpawnHeroes(bool enabled)
        {
            yield return new SpawnHeroesChanged(this, enabled);
        }

        public IEnumerable<IEvent> SpawnPlayerAvatar(IStorage<Creature> storage)
        {
            var position = _random.From(_playersSpawnPositions);

            var container = new Container<Creature>();
            
            var events = _creatureFactory.Create(storage.ComposeWith(container), position, _playersAvatarProperties);

            foreach (var e in events) { yield return e;}

            yield return new PlayerAvatarSpawned(this, container);
        }

        public IEnumerable<IEvent> SpawnHero()
        {
            var position = _random.From(_heroesSpawnPositions);

            var events = _heroFactory.Create(position, _heroesCreatureProperties);

            var clock = _clockContainer.Item;

            return events.ConcatItem(new HeroSpawnActivationTimeChanged(this, clock.GetCurrentTime()));
        }

        internal void SetSpawnHeroes(bool enabled)
        {
            _spawnHeroes = enabled;
        }

        internal void SetHeroSpawnActivationTime(TimeSpan activationTime)
        {
            _heroSpawnCooldown.SetLastActivationTime(activationTime);
        }
    }
}
