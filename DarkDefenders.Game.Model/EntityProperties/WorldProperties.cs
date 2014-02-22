using System.Collections.Generic;
using System.Collections.ObjectModel;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Game.Model.EntityProperties
{
    public class WorldProperties: SlowValueObject
    {
        public ReadOnlyCollection<Vector> HeroesSpawnPositions { get; private set; }
        public ReadOnlyCollection<Vector> PlayersSpawnPositions { get; private set; }

        public WorldProperties(IEnumerable<Vector> playersSpawnPositions, IEnumerable<Vector> heroesSpawnPositions)
        {
            HeroesSpawnPositions = heroesSpawnPositions.AsReadOnly();
            PlayersSpawnPositions = playersSpawnPositions.AsReadOnly();
        }
    }
}