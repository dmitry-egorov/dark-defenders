using System;
using System.Diagnostics;
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

        public Vector LimitMomentum(Vector momentum, Box boundingBox)
        {
            var px = momentum.X;
            var py = momentum.Y;

            if (px > 0)
            {
                if (IsTouchingAWallToTheRight(boundingBox))
                {
                    Debug.WriteLine("MR");
                    px = 0;
                }
            }
            else if (px < 0)
            {
                if (IsTouchingAWallToTheLeft(boundingBox))
                {
                    Debug.WriteLine("ML");
                    px = 0;
                }
            }

            if (py > 0)
            {
                if (IsTouchingTheCeiling(boundingBox))
                {
                    Debug.WriteLine("MC");
                    py = 0;
                }
            }
            else if (py < 0)
            {
                if (IsTouchingTheGround(boundingBox))
                {
                    Debug.WriteLine("MF");
                    py = 0;
                }
            }

            return Vector.XY(px, py);
        }

        public bool IsAdjacentToAWall(Box box)
        {
            return IsTouchingAWallToTheRight(box)
                || IsTouchingAWallToTheLeft(box)
                || IsTouchingTheCeiling(box)
                || IsTouchingTheGround(box);
        }

        public bool IsInTheAir(Box box)
        {
            return !IsTouchingTheGround(box);
        }

        public Vector ApplyPositionChange(Box box, Vector positionDelta)
        {
            Vector horizontalAdjustment;
            var horizontalPositionAdjusted = ApplyHorizontalPositionChange(box, positionDelta, out horizontalAdjustment);

            Vector verticalAdjustment;
            var verticalPositionAdjusted = ApplyVerticalPositionChange(box, positionDelta, out verticalAdjustment);

            if (verticalPositionAdjusted && horizontalPositionAdjusted)
            {
                var horizontalDelta = (horizontalAdjustment - box.Center).LengthSquared();
                var verticalDelta = (verticalAdjustment - box.Center).LengthSquared();
                Debug.WriteLine("H: " + horizontalAdjustment + "; V: " + verticalAdjustment);

                return horizontalDelta < verticalDelta ? horizontalAdjustment : verticalAdjustment;
            }

            if (horizontalPositionAdjusted)
            {
                Debug.WriteLine("H: " + horizontalAdjustment);
                return horizontalAdjustment;
            }

            if (verticalPositionAdjusted)
            {
                Debug.WriteLine("V: " + verticalAdjustment);
                return verticalAdjustment;
            }

            return box.Center + positionDelta;
        }

        private bool ApplyHorizontalPositionChange(Box box, Vector positionDelta, out Vector adjustedPosition)
        {
            var dx = positionDelta.X;
            var dy = positionDelta.Y;

            if (dx == 0.0)
            {
                adjustedPosition = Vector.Zero;
                return false;
            }

            var sign = Math.Sign(dx);
            var slopeY = dy / dx;

            var startBoundX = box.Center.X + sign * box.WidthRadius;
            var startBoundY = box.Center.Y;
            var endBoundX = startBoundX + dx;

            var xStart = ((sign == 1) ? startBoundX.TolerantCeiling() : startBoundX.TolerantFloor()).ToInt();
            var xEnd = ((sign == 1) ? endBoundX.PrevInteger() : endBoundX.NextInteger()).ToInt();

            for (var x = xStart; (x - xEnd) * sign <= 0; x += sign)
            {
                var checkX = (sign == 1) ? x : x - 1;
                var y = slopeY * (x - startBoundX) + startBoundY;
                if (!IsTouchingVerticalWalls(checkX, y, box.HeightRadius))
                {
                    continue;
                }

                adjustedPosition = Vector.XY(x - sign * box.WidthRadius, y);
                return true;
            }

            adjustedPosition = Vector.Zero;
            return false;
        }

        private bool ApplyVerticalPositionChange(Box box, Vector positionDelta, out Vector adjustedPosition)
        {
            var dx = positionDelta.X;
            var dy = positionDelta.Y;

            if (dy == 0.0)
            {
                adjustedPosition = Vector.Zero;
                return false;
            }

            var sign = Math.Sign(dy);
            var slopeX = dx / dy;

            var startBoundY = box.Center.Y + sign * box.HeightRadius;
            var startBoundX = box.Center.X;
            var endBoundY = startBoundY + dy;

            var yStart = (sign == 1 ? startBoundY.TolerantCeiling() : startBoundY.TolerantFloor()).ToInt();
            var yEnd   = (sign == 1 ? endBoundY.PrevInteger(): endBoundY.NextInteger()).ToInt();

            for (var y = yStart; (y - yEnd) * sign <= 0; y += sign)
            {
                var checkY = (sign == 1) ? y : y - 1;
                var x = slopeX * (y - startBoundY) + startBoundX;
                if (!IsTouchingHorizontalWallsAt(checkY, x, box.WidthRadius))
                {
                    continue;
                }

                adjustedPosition = Vector.XY(x, y - sign * box.HeightRadius);
                return true;
            }

            adjustedPosition = Vector.Zero;
            return false;
        }

        private bool IsTouchingTheGround(Box box)
        {
            var bottom = box.Center.Y - box.HeightRadius;

            var y = bottom.PrevInteger().ToInt();

            return IsTouchingHorizontalWallsAt(y, box.Center.X, box.WidthRadius);
        }

        private bool IsTouchingTheCeiling(Box box)
        {
            var top = box.Center.Y + box.HeightRadius;

            var y = top.TolerantFloor().ToInt();

            return IsTouchingHorizontalWallsAt(y, box.Center.X, box.WidthRadius);
        }

        private bool IsTouchingAWallToTheLeft(Box box)
        {
            var left = box.Center.X - box.WidthRadius;

            var x = left.PrevInteger().ToInt();
            
            return IsTouchingVerticalWalls(x, box.Center.Y, box.HeightRadius);
        }

        private bool IsTouchingAWallToTheRight(Box box)
        {
            var right = box.Center.X + box.WidthRadius;

            var x = right.TolerantFloor().ToInt();

            return IsTouchingVerticalWalls(x, box.Center.Y, box.HeightRadius);
        }

        private bool IsTouchingHorizontalWallsAt(int y, double xCenter, double widthRadius)
        {
            var xStart = (xCenter - widthRadius).TolerantFloor().ToInt();
            var xEnd = (xCenter + widthRadius).PrevInteger().ToInt();

            for (var x = xStart; x <= xEnd; x++)
            {
                if (_map[x, y] == Tile.Solid)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsTouchingVerticalWalls(int x, double yCenter, double heightRadius)
        {
            var yStart = (yCenter - heightRadius).TolerantFloor().ToInt();
            var yEnd = (yCenter + heightRadius).PrevInteger().ToInt();

            for (var y = yStart; y <= yEnd; y++)
            {
                if (_map[x, y] == Tile.Solid)
                {
                    return true;
                }
            }

            return false;
        }
    }
}