using System.Runtime.CompilerServices;
using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities
{
    [UsedImplicitly]
    public class Terrain : Entity<Terrain, ITerrainEvents>, ITerrainEvents
    {
        private readonly IResources<Map<Tile>> _mapResources;
        
        private Map<Tile> _map;

        public Terrain(IResources<Map<Tile>> mapResources)
        {
            _mapResources = mapResources;
        }

        public void Create(string mapId)
        {
            CreationEvent(x => x.Created(mapId));
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

        void ITerrainEvents.Created(string mapId)
        {
            _map = _mapResources[mapId];
        }
    }
}