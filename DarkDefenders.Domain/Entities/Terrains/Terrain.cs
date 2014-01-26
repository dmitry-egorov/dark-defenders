using System.Runtime.CompilerServices;
using DarkDefenders.Dtos.Entities.Terrains;
using DarkDefenders.Dtos.Other;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Entities.Terrains
{
    internal class Terrain : Entity<TerrainId>
    {
        private readonly Map<Tile> _map;

        public Terrain(Map<Tile> map)
        {
            _map = map;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AnySolidWallsAt(Axis axis, int mainStart, int mainEnd, int other)
        {
            return _map.IsAnyAtLine(axis, mainStart, mainEnd, other, Tile.Solid);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AnyOpenWallsAt(Axis axis, int mainStart, int mainEnd, int other)
        {
            return _map.IsAnyAtLine(axis, mainStart, mainEnd, other, Tile.Open);
        }

        public bool IsSolidWallAt(int x, int y)
        {
            return _map[x, y] == Tile.Solid;
        }
    }
}