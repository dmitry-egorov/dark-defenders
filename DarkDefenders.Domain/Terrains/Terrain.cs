using System.Runtime.CompilerServices;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Terrains.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Terrains
{
    public class Terrain: RootBase<TerrainId, ITerrainEventsReciever, ITerrainEvent>, ITerrainEventsReciever
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AnySolidWallsAt(Axis axis, int mainStart, int mainEnd, int other)
        {
            return _terrain.IsAnyAtLine(axis, mainStart, mainEnd, other, Tile.Solid);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AnyOpenWallsAt(Axis axis, int mainStart, int mainEnd, int other)
        {
            return _terrain.IsAnyAtLine(axis, mainStart, mainEnd, other, Tile.Open);
        }

        public bool IsSolidWallAt(int x, int y)
        {
            return _terrain[x, y] == Tile.Solid;
        }

        internal Terrain(TerrainId id, Map<Tile> terrain) : base(id)
        {
            _terrain = terrain;
        }

        private readonly Map<Tile> _terrain;
    }
}