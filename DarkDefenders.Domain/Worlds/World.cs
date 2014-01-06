using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Worlds
{
    public class World : RootBase<WorldId, IWorldEventsReciever, IWorldEvent>, IWorldEventsReciever
    {
        public IEnumerable<IDomainEvent> SpawnPlayerAvatar(CreatureId creatureId)
        {
            var events = _creatureFactory.Create(creatureId, _clock.Id, Id, _playersSpawnPosition, _playerAvatarProperties);

            foreach (var e in events) { yield return e; }

            yield return new PlayerAvatarSpawned(Id, creatureId);
        }

        public IEnumerable<IDomainEvent> SpawnHeroes()
        {
            if (_heroSpawnCooldown.IsInEffect())
            {
                yield break;
            }

            var events = SpawnHero();

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IDomainEvent> SpawnHero()
        {
            var heroCreatureId = new CreatureId();

            var events = _creatureFactory.Create(heroCreatureId, _clock.Id, Id, _heroesSpawnPosition, _heroesCreatureProperties);

            foreach (var e in events) { yield return e; }

            yield return new HeroesSpawned(Id, _clock.GetCurrentTime(), heroCreatureId);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AnySolidWallsAt(Axis axis, int mainStart, int mainEnd, int other)
        {
            return _terrain.IsAnyAtLine(axis, mainStart, mainEnd, other, Tile.Solid);
        }

        public void Recieve(HeroesSpawned heroesSpawned)
        {
            _heroSpawnCooldown.SetLastActivationTime(heroesSpawned.Time);
        }

        public void Recieve(PlayerAvatarSpawned playerAvatarSpawned)
        {
            
        }

        internal World
            (
                WorldId id, 
                Clock clock, 
                CreatureFactory creatureFactory, 
                Map<Tile> terrain, 
                Vector playersSpawnPosition, 
                CreatureProperties playerAvatarProperties, 
                Vector heroesSpawnPosition, 
                TimeSpan heroesSpawnCooldown, 
                CreatureProperties heroesCreatureProperties
            ) : base(id)
        {
            _creatureFactory = creatureFactory;
            _heroesSpawnPosition = heroesSpawnPosition;
            _heroesCreatureProperties = heroesCreatureProperties;
            _playerAvatarProperties = playerAvatarProperties;

            _terrain = terrain;
            _playersSpawnPosition = playersSpawnPosition;

            _clock = clock;
            _heroSpawnCooldown = new Cooldown(clock, heroesSpawnCooldown);
        }


        private readonly CreatureFactory _creatureFactory;

        private readonly Vector _playersSpawnPosition;
        private readonly CreatureProperties _playerAvatarProperties;
        private readonly Map<Tile> _terrain;
        private readonly Clock _clock;
        private readonly Cooldown _heroSpawnCooldown;
        private readonly Vector _heroesSpawnPosition;
        private readonly CreatureProperties _heroesCreatureProperties;
    }
}
