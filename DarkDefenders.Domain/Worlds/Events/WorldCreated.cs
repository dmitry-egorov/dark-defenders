using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Terrains;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Worlds.Events
{
    public class WorldCreated : EventBase<WorldId, WorldCreated>, IDomainEvent
    {
        public ClockId ClockId { get; private set; }
        public TerrainId TerrainId { get; private set; }
        public ReadOnlyCollection<Vector> PlayersSpawnPosition { get; private set; }
        public CreatureProperties PlayersAvatarProperties { get; private set; }
        public ReadOnlyCollection<Vector> HeroesSpawnPositions { get; private set; }
        public TimeSpan HeroesSpawnCooldown { get; private set; }
        public CreatureProperties HeroesCreatureProperties { get; private set; }

        public WorldCreated
        (
            WorldId worldId, 
            ClockId clockId, 
            TerrainId terrainId, 
            IEnumerable<Vector> playersSpawnPosition, 
            CreatureProperties playersAvatarProperties, 
            IEnumerable<Vector> heroesSpawnPositions, 
            TimeSpan heroesSpawnCooldown, 
            CreatureProperties heroesCreatureProperties
        )
        : base(worldId)
        {
            ClockId = clockId;
            TerrainId = terrainId;
            HeroesCreatureProperties = heroesCreatureProperties;
            HeroesSpawnCooldown = heroesSpawnCooldown;
            HeroesSpawnPositions = heroesSpawnPositions.AsReadOnly();
            PlayersAvatarProperties = playersAvatarProperties;
            PlayersSpawnPosition = playersSpawnPosition.AsReadOnly();
        }

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}