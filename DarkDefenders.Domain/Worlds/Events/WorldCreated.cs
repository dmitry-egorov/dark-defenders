using System;
using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Other;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Worlds.Events
{
    public class WorldCreated : EventBase<WorldId, WorldCreated>, IDomainEvent
    {
        public ClockId ClockId { get; private set; }
        public Map<Tile> Map { get; private set; }
        public Vector PlayersSpawnPosition { get; private set; }
        public CreatureProperties PlayersAvatarProperties { get; private set; }
        public Vector HeroesSpawnPosition { get; private set; }
        public TimeSpan HeroesSpawnCooldown { get; private set; }
        public CreatureProperties HeroesCreatureProperties { get; private set; }

        public WorldCreated(WorldId worldId, ClockId clockId, Map<Tile> map, Vector playersSpawnPosition, CreatureProperties playersAvatarProperties, Vector heroesSpawnPosition, TimeSpan heroesSpawnCooldown, CreatureProperties heroesCreatureProperties)
            : base(worldId)
        {
            HeroesCreatureProperties = heroesCreatureProperties;
            HeroesSpawnCooldown = heroesSpawnCooldown;
            HeroesSpawnPosition = heroesSpawnPosition;
            PlayersAvatarProperties = playersAvatarProperties;
            ClockId = clockId;
            Map = map;
            PlayersSpawnPosition = playersSpawnPosition;
        }

        protected override string ToStringInternal()
        {
            return "World created: {0}, {1}, {2}, {3}, {4}, {5}, {6}"
                   .FormatWith(RootId, ClockId, PlayersSpawnPosition, PlayersAvatarProperties, HeroesSpawnPosition, HeroesSpawnCooldown, HeroesCreatureProperties);
        }

        protected override bool EventEquals(WorldCreated other)
        {
            return ClockId.Equals(other.ClockId)
                && Map.Equals(other.Map)
                && PlayersSpawnPosition.Equals(other.PlayersSpawnPosition)
                && PlayersAvatarProperties.Equals(other.PlayersAvatarProperties)
                && HeroesSpawnPosition.Equals(other.HeroesSpawnPosition)
                && HeroesSpawnCooldown.Equals(other.HeroesSpawnCooldown)
                && HeroesCreatureProperties.Equals(other.HeroesCreatureProperties)
                ;
        }

        protected override int GetEventHashCode()
        {
            var hashCode = Map.GetHashCode();
            hashCode = (hashCode * 397) ^ PlayersSpawnPosition.GetHashCode();
            hashCode = (hashCode * 397) ^ ClockId.GetHashCode();
            return hashCode;
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}