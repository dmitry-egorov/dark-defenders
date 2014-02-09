using System.Collections.Generic;
using System.Collections.ObjectModel;
using DarkDefenders.Domain.Data.Other;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Files
{
    public class TerrainData
    {
        public Map<Tile> Map { get; private set; }
        public ReadOnlyCollection<Vector> PlayerSpawns { get; private set; }
        public ReadOnlyCollection<Vector> HeroSpawns { get; private set; }

        public TerrainData(Map<Tile> map, IEnumerable<Vector> playerSpawns, IEnumerable<Vector> heroSpawns)
        {
            Map = map;
            PlayerSpawns = playerSpawns.AsReadOnly().ShouldNotBeEmpty("playerSpawns");
            HeroSpawns = heroSpawns.AsReadOnly().ShouldNotBeEmpty("heroSpawns");
        }
    }
}