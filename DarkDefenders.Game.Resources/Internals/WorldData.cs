using System.Collections.Generic;
using System.Collections.ObjectModel;
using DarkDefenders.Game.Model.Other;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Game.Resources.Internals
{
    public class WorldData
    {
        public Map<Tile> Map { get; private set; }
        public ReadOnlyCollection<Vector> PlayerSpawns { get; private set; }
        public ReadOnlyCollection<Vector> HeroSpawns { get; private set; }

        public WorldData(Map<Tile> map, IEnumerable<Vector> playerSpawns, IEnumerable<Vector> heroSpawns)
        {
            Map = map;
            PlayerSpawns = playerSpawns.AsReadOnly().ShouldNotBeEmpty("playerSpawns");
            HeroSpawns = heroSpawns.AsReadOnly().ShouldNotBeEmpty("heroSpawns");
        }
    }
}