using System.Linq;
using System.Runtime.CompilerServices;
using DarkDefenders.Game.Model.Events;
using DarkDefenders.Game.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using JetBrains.Annotations;

namespace DarkDefenders.Game.Model.Entities
{
    public class AccessabilityMap
    {
        private readonly Map<bool> _map;

        public AccessabilityMap(Map<bool> map)
        {
            _map = map;
        }

        public bool IsAccessible(int x, int y)
        {
            return _map[x, y];
        }

        public bool IsAccessible(DiscreteAxisAlignedLine line)
        {
            return _map.ValuesOn(line).All(x => x);
        }
    }

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
        public bool AnyOpenWallsAt(DiscreteAxisAlignedLine line)
        {
            return _map.ValuesOn(line).Any(x => x == Tile.Open);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsTouchingWalls(Box box, Direction direction)
        {
            var bound = box.GetBound(direction);

            var axisDirection = direction.AxisDirection();

            return AdjacentToAWall(bound, axisDirection);
        }

        void ITerrainEvents.Created(string mapId)
        {
            _map = _mapResources[mapId];
        }

        public Vector IntersectMovingBox(Box box, Vector positionDelta)
        {
            var center = box.GetPosition();

            var horizontalPosition = Intersect(Axis.X, box, positionDelta);

            var verticalPosition   = Intersect(Axis.Y, box, positionDelta);
            
            var horizontalDelta = (horizontalPosition - center).Length();
            var verticalDelta   = (verticalPosition   - center).Length();

            return
            horizontalDelta < verticalDelta
            ? horizontalPosition
            : verticalPosition;
        }

        private Vector Intersect(Axis axis, Box box, Vector positionDelta)
        {
            var axisDirection = positionDelta.AxisDirection(axis);
            var direction = axis.AbsoluteDirection(axisDirection);

            var snappedBoxes = box.GetSnappedBoxes(axis, positionDelta);

            var adjacentBoxes = 
            snappedBoxes
            .Where(snappedBox => AdjacentToAWall(snappedBox.GetBound(direction), axisDirection));

            foreach (var adjacentBox in adjacentBoxes)
            {
                return adjacentBox.GetPosition();
            }

            return box.GetPosition() + positionDelta;
        }

        private bool AdjacentToAWall(AxisAlignedLine line, AxisDirection axisDirection)
        {
            var discreteLine = line.DiscreteExpanded(axisDirection);

            return _map.ValuesOn(discreteLine).Any(x => x == Tile.Solid);
        }
    }
}