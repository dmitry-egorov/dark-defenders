using System;
using DarkDefenders.Domain.Other;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Worlds
{
    internal class Terrain
    {
        private readonly Map<Tile> _map;

        public Terrain(Map<Tile> map)
        {
            _map = map;
        }

        public Vector LimitMomentum(Vector momentum, Circle boundingCircle)
        {
            var px = momentum.X;
            var py = momentum.Y;

            if (px > 0)
            {
                if (IsFacingAWallToTheRight(boundingCircle))
                {
                    px = 0;
                }
            }
            else if (px < 0)
            {
                if (IsFacingAWallToTheLeft(boundingCircle))
                {
                    px = 0;
                }
            }

            if (py > 0)
            {
                if (IsAbutingTheCeiling(boundingCircle))
                {
                    py = 0;
                }
            }
            else if (py < 0)
            {
                if (IsStandingOnTheGround(boundingCircle))
                {
                    py = 0;
                }
            }

            return Vector.XY(px, py);
        }

        public bool IsAdjacentToAWall(Circle circle)
        {
            return IsFacingAWallToTheRight(circle)
                || IsFacingAWallToTheLeft(circle)
                || IsAbutingTheCeiling(circle)
                || IsStandingOnTheGround(circle);
        }

        public Vector LimitPositionChange(Circle circle, Vector positionChange)
        {
            var radius = circle.Radius;

            var lengthSquared = positionChange.LengthSquared();
            var radiusSquared = radius * radius;

            if (lengthSquared > radiusSquared)
            {
                var length = Math.Sqrt(lengthSquared);
                var step = positionChange * (radius / length);

                var fullStepsCount = (int) (length / radius);

                var nextCircle = circle;

                for (var i = 0; i < fullStepsCount; i++)
                {
                    nextCircle = nextCircle.Move(step);

                    var limitedPosition = LimitPosition(nextCircle);

                    if (limitedPosition != nextCircle.Position)
                    {
                        return limitedPosition - circle.Position;
                    }
                }
            }

            var newCircle = circle.Move(positionChange);

            var newPosition = LimitPosition(newCircle);

            return newPosition - circle.Position;
        }

        public bool IsInTheAir(Circle circle)
        {
            return !IsStandingOnTheGround(circle);
        }

        private Vector LimitPosition(Circle newCircle)
        {
            var position = newCircle.Position;
            var radius = newCircle.Radius;
            var x = position.X;
            var y = position.Y;

            if (IsFacingAWallToTheRight(newCircle))
            {
                x = ((x + radius).Floor() - radius);
            }
            else if (IsFacingAWallToTheLeft(newCircle))
            {
                x = ((x - radius).PrevInteger() + 1.0 + radius);
            }

            if (IsAbutingTheCeiling(newCircle))
            {
                y = ((y + radius).Floor() - radius);
            }
            else if (IsStandingOnTheGround(newCircle))
            {
                y = ((y - radius).PrevInteger() + 1.0 + radius);
            }

            return Vector.XY(x, y);
        }

        private bool IsStandingOnTheGround(Circle circle)
        {
            var bottom = circle.Position.Y - circle.Radius;

            var x = circle.Position.X.ToInt();
            var y = bottom.PrevInteger().ToInt();

            return bottom <= 0.0 || _map[x, y] == Tile.Solid;
        }

        private bool IsAbutingTheCeiling(Circle circle)
        {
            var top = circle.Position.Y + circle.Radius;

            var x = circle.Position.X.ToInt();
            var y = top.Floor().ToInt();

            return _map[x, y] == Tile.Solid;
        }

        private bool IsFacingAWallToTheLeft(Circle circle)
        {
            var left = circle.Position.X - circle.Radius;

            var x = left.PrevInteger().ToInt();
            var y = circle.Position.Y.ToInt();

            return _map[x, y] == Tile.Solid;
        }

        private bool IsFacingAWallToTheRight(Circle circle)
        {
            var right = circle.Position.X + circle.Radius;

            var x = right.Floor().ToInt();
            var y = circle.Position.Y.ToInt();

            return _map[x, y] == Tile.Solid;
        }
    }
}