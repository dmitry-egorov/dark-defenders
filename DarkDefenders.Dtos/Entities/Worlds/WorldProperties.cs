using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DarkDefenders.Dtos.Entities.Creatures;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Dtos.Entities.Worlds
{
    public class WorldProperties
    {
        public CreatureProperties PlayersAvatarProperties { get; private set; }
        public ReadOnlyCollection<Vector> HeroesSpawnPositions { get; set; }
        public TimeSpan HeroesSpawnCooldown { get; set; }
        public CreatureProperties HeroesCreatureProperties { get; set; }
        public ReadOnlyCollection<Vector> PlayersSpawnPositions { get; private set; }

        public WorldProperties(IEnumerable<Vector> playersSpawnPositions, CreatureProperties playersAvatarProperties, IEnumerable<Vector> heroesSpawnPositions, TimeSpan heroesSpawnCooldown, CreatureProperties heroesCreatureProperties)
        {
            PlayersAvatarProperties = playersAvatarProperties;
            HeroesSpawnPositions = heroesSpawnPositions.AsReadOnly();
            HeroesSpawnCooldown = heroesSpawnCooldown;
            HeroesCreatureProperties = heroesCreatureProperties;
            PlayersSpawnPositions = playersSpawnPositions.AsReadOnly();
        }
    }
}