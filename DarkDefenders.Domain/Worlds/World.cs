using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Worlds
{
    public class World : RootBase<WorldId, IWorldEventsReciever, IWorldEvent>, IWorldEventsReciever
    {
        public double TimeSeconds { get; private set; }
        public double ElapsedSeconds { get; private set; }

        public IEnumerable<IWorldEvent> UpdateWorldTime(double elapsed)
        {
            var newTime = TimeSeconds + elapsed;

            yield return new WorldTimeUpdated(Id, newTime, elapsed);
        }

        public Vector GetSpawnPosition()
        {
            return _spawnPosition;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsInTheAir(Circle boundingCircle)
        {
            return boundingCircle.IsAboveHorizontalAxis();
        }

        public Vector AdjustCirclePosition(Circle circle)
        {
            var position = circle.Position;
            var radius = circle.Radius;
            var x = position.X;
            var y = position.Y;
            var lx = _dimensions.Width;
            var ly = _dimensions.Height;

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

        public Vector LimitMomentum(Vector momentum, Circle boundingCircle)
        {
            var px = momentum.X;
            var py = momentum.Y;

            var x = boundingCircle.Position.X;
            var y = boundingCircle.Position.Y;
            var radius = boundingCircle.Radius;
            var lx = _dimensions.Width;
            var ly = _dimensions.Height;

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

            var dx = _dimensions.Width - radius;
            var dy = _dimensions.Height - radius;
            return x >= dx 
                || x <= radius 
                || y >= dy 
                || y <= radius;
        }

        public void Recieve(WorldTimeUpdated worldTimeUpdated)
        {
            TimeSeconds = worldTimeUpdated.NewTime;
            ElapsedSeconds = worldTimeUpdated.Elapsed;
        }

        internal World(WorldId id, Dimensions dimensions, Vector spawnPosition) : base(id)
        {
            _dimensions = dimensions;
            _spawnPosition = spawnPosition;
            TimeSeconds = 0.0;
        }

        private readonly Dimensions _dimensions;
        private readonly Vector _spawnPosition;
    }
}
