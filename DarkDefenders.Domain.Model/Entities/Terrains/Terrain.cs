using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DarkDefenders.Domain.Model.Entities.Terrains.Events;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities.Terrains
{
    [UsedImplicitly]
    public class Terrain : Entity<Terrain>
    {
        private readonly IStorage<Terrain> _storage;
        private readonly IResources<Map<Tile>> _mapResources;
        
        private Map<Tile> _map;

        public Terrain(IStorage<Terrain> storage, IResources<Map<Tile>> mapResources)
        {
            _storage = storage;
            _mapResources = mapResources;
        }

        public IEnumerable<IEvent> Create(string mapId)
        {
            yield return new TerrainCreated(this, _storage, mapId);
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

        internal void Created(string mapId)
        {
            _map = _mapResources[mapId];
        }
    }
}