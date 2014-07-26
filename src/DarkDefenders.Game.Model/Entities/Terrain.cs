using System.Runtime.CompilerServices;
using DarkDefenders.Game.Model.Events;
using DarkDefenders.Game.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using JetBrains.Annotations;

namespace DarkDefenders.Game.Model.Entities
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsTouchingWallsAt(Axis axis, double mainCenter, double radius, int other)
        {
            return _map.IsTouchingWallsAt(axis, mainCenter, radius, other, Tile.Solid);
        }

        void ITerrainEvents.Created(string mapId)
        {
            _map = _mapResources[mapId];
        }

        public Vector IntersectMovingBox(Vector center, Vector positionDelta, Box boundingBox)
        {
            return _map.IntersectMovingBox(center, positionDelta, boundingBox, Tile.Solid);
        }
    }
}