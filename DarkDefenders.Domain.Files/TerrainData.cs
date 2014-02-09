using System.Collections.Generic;
using System.Collections.ObjectModel;
using DarkDefenders.Domain.Data.Other;
using Infrastructure.Data;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Files
{
    public class TerrainData
    {
        public MapData<Tile> Map { get; private set; }
        public ReadOnlyCollection<VectorData> PlayerSpawns { get; private set; }
        public ReadOnlyCollection<VectorData> HeroSpawns { get; private set; }

        public TerrainData(MapData<Tile> map, IEnumerable<VectorData> playerSpawns, IEnumerable<VectorData> heroSpawns)
        {
            Map = map;
            PlayerSpawns = playerSpawns.AsReadOnly().ShouldNotBeEmpty("playerSpawns");
            HeroSpawns = heroSpawns.AsReadOnly().ShouldNotBeEmpty("heroSpawns");
        }
    }
}