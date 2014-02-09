using System;
using System.Collections.Generic;
using System.Linq;
using DarkDefenders.Domain.Entities.Creatures;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Entities.Worlds
{
    public class WorldProperties: SlowValueObject
    {
        public CreatureProperties PlayersAvatarProperties { get; private set; }
        public List<Vector> HeroesSpawnPositions { get; private set; }
        public TimeSpan HeroesSpawnCooldown { get; private set; }
        public CreatureProperties HeroesCreatureProperties { get; private set; }
        public List<Vector> PlayersSpawnPositions { get; private set; }

        public WorldProperties(IEnumerable<Vector> playersSpawnPositions, CreatureProperties playersAvatarProperties, IEnumerable<Vector> heroesSpawnPositions, TimeSpan heroesSpawnCooldown, CreatureProperties heroesCreatureProperties)
        {
            PlayersAvatarProperties = playersAvatarProperties;
            HeroesSpawnPositions = heroesSpawnPositions.ToList();
            HeroesSpawnCooldown = heroesSpawnCooldown;
            HeroesCreatureProperties = heroesCreatureProperties;
            PlayersSpawnPositions = playersSpawnPositions.ToList();
        }
    }
}