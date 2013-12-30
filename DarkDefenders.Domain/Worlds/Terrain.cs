using Infrastructure.Math;

namespace DarkDefenders.Domain.Worlds
{
    internal class Terrain
    {
        private readonly Map _map;

        public Terrain(Map map)
        {
            _map = map;
        }

        public Vector LimitMomentum(Vector momentum, Circle boundingCircle)
        {
            var px = momentum.X;
            var py = momentum.Y;

            var position = boundingCircle.Position;
            var x = position.X;
            var y = position.Y;

            var radius = boundingCircle.Radius;

            var dimensions = _map.Dimensions;
            var lx = dimensions.Width;
            var ly = dimensions.Height;

            var dx = lx - radius;
            if (px >= 0)
            {
                if (x >= dx)
                {
                    px = 0;
                }
            }
            else
            {
                if (x <= radius)
                {
                    px = 0;
                }
            }

            var dy = ly - radius;
            if (py >= 0)
            {
                if (y >= dy)
                {
                    py = 0;
                }
            }
            else
            {
                if (y <= radius)
                {
                    py = 0;
                }
            }

            return Vector.XY(px, py);
        }

        public bool IsAdjacentToAWall(Circle circle)
        {
            var position = circle.Position;
            var radius = circle.Radius;
            var x = position.X;
            var y = position.Y;

            var dimensions = _map.Dimensions;

            var dx = dimensions.Width - radius;
            var dy = dimensions.Height - radius;
            return x >= dx
                || x <= radius
                || y >= dy
                || y <= radius;
        }

        public Vector AdjustCirclePosition(Circle circle)
        {
            var position = circle.Position;
            var radius = circle.Radius;
            var x = position.X;
            var y = position.Y;
            
            var dimensions = _map.Dimensions;
            var lx = dimensions.Width;
            var ly = dimensions.Height;

            var dx = lx - radius;

            if (x > dx)
            {
                x = dx;
            }
            else if (x < radius)
            {
                x = radius;
            }

            var dy = ly - radius;
            if (y > dy)
            {
                y = dy;
            }
            else if (y < radius)
            {
                y = radius;
            }

            return Vector.XY(x, y);
        }
    }
}