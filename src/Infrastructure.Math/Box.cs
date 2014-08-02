using System;
using System.Collections.Generic;

namespace Infrastructure.Math
{
    public struct Box
    {
        private readonly Vector _position;
        private readonly Bounds _bounds;

        public Box(Vector position, Bounds bounds)
        {
            _position = position;
            _bounds = bounds;
        }

        public Box MovedTo(Vector newPosition)
        {
            return new Box(newPosition, _bounds);
        }

        public Vector GetPosition()
        {
            return _position;
        }

        public int BoundSlot(Direction direction)
        {
            var coordinate = GetBoundCoordinate(direction);

            return coordinate.ToInt();
        }

        public int NextSlot(Direction direction)
        {
            return BoundSlot(direction) + direction.GetIncrement();
        }

        private double GetBoundCoordinate(Direction direction)
        {
            var axis = direction.Axis();
            return _position.CoordinateFor(axis) + direction.GetIncrement() * _bounds.RadiusFor(axis);
        }

        public AxisAlignedLine GetBound(Direction direction)
        {
            var directionAxis = direction.Axis();
            var orientation   = directionAxis.Other();
            var level         = GetBoundCoordinate(direction);
            var radius        = _bounds.RadiusFor(orientation);
            var start         = _position.CoordinateFor(orientation) - radius;
            var length        = radius * 2.0;
            var origin        = Vector.ByAxis(orientation, start, level);

            return new AxisAlignedLine(orientation, origin, length);
        }

        public IEnumerable<Box> GetSnappedBoxes(Axis axis, Vector positionDelta)
        {
            var axisDirection = positionDelta.AxisDirection(axis);
            if (axisDirection == AxisDirection.None)
            {
                yield break;
            }

            var direction = axis.AbsoluteDirection(axisDirection);

            var initialLine = GetBound(direction);

            var slope = positionDelta.SlopeFor(axis);

            var boundPositionOffset = GetBoundPositionOffset(direction);

            for (var i = 0;; i++)
            {
                var currentLine = initialLine.SnapToNextDiscreteLevel(axisDirection, i, slope);
                var currentOffset = currentLine.GetOrigin() - initialLine.GetOrigin();

                if (currentOffset.LengthSquared() >= positionDelta.LengthSquared())
                {
                    yield break;
                }

                var newOrigin = currentLine.GetOrigin() - boundPositionOffset;
                yield return new Box(newOrigin, _bounds);
            }
        }

        private Vector GetBoundPositionOffset(Direction direction)
        {
            var widthRadius  = _bounds.WidthRadius;
            var heightRadius = _bounds.HeightRadius;
            switch (direction)
            {
                case Direction.None:
                    return Vector.Zero;
                case Direction.Left:
                    return Vector.XY(-widthRadius, -heightRadius);
                case Direction.Right:
                    return Vector.XY( widthRadius, -heightRadius);
                case Direction.Top:
                    return Vector.XY(-widthRadius,  heightRadius);
                case Direction.Bottom:
                    return Vector.XY(-widthRadius, -heightRadius);
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }
    }
}