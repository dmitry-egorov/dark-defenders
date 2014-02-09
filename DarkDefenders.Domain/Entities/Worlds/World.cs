using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DarkDefenders.Domain.Data.Entities.Creatures;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.Heroes;
using DarkDefenders.Domain.Entities.Other;
using DarkDefenders.Domain.Entities.Worlds.Events;
using DarkDefenders.Domain.Factories;

using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Entities.Worlds
{
    public class World : Entity<World>
    {
        private readonly CreatureFactory _creatureFactory;
        private readonly HeroFactory _heroFactory;

        private readonly Clock _clock;
        private readonly Random _random;

        private readonly ReadOnlyCollection<Vector> _playersSpawnPositions;
        private readonly CreatureProperties _playersAvatarProperties;
        private readonly Cooldown _heroSpawnCooldown;
        private readonly ReadOnlyCollection<Vector> _heroesSpawnPositions;
        private readonly CreatureProperties _heroesCreatureProperties;

        private bool _spawnHeroes = true;

        internal World
        (
            HeroFactory heroFactory, 
            CreatureFactory creatureFactory, 
            Clock clock, 
            Random random, 
            WorldProperties properties
        )
        {
            _creatureFactory = creatureFactory;
            _heroesSpawnPositions = properties.HeroesSpawnPositions.AsReadOnly();
            _heroesCreatureProperties = properties.HeroesCreatureProperties;
            _heroFactory = heroFactory;
            _random = random;
            _playersAvatarProperties = properties.PlayersAvatarProperties;

            _playersSpawnPositions = properties.PlayersSpawnPositions.AsReadOnly();

            _clock = clock;
            _heroSpawnCooldown = new Cooldown(clock, properties.HeroesSpawnCooldown);
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

        public ICreation<Creature> SpawnPlayerAvatar()
        {
            var position = _random.ElementFrom(_playersSpawnPositions);

            var creation = _creatureFactory.Create(position, _playersAvatarProperties);

            return creation.ConcatEvent(new PlayerAvatarSpawned(this, creation));
        }

        public ICreation<Hero> SpawnHero()
        {
            var position = _random.ElementFrom(_heroesSpawnPositions);

            var events = _heroFactory.Create(position, _heroesCreatureProperties);

            var currentTime = _clock.GetCurrentTime();

            return events.ConcatEvent(new HeroSpawnActivationTimeChanged(this, currentTime));
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
