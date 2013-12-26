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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector GetGravityForce(double mass)
        {
            return Vector.XY(0, -mass * GravityAcceleration);
        }

        public Vector AdjustCirclePosition(Circle circle)
        {
            var position = circle.Position;
            var radius = circle.Radius;
            var x = position.X;
            var y = position.Y;

            var dr = 1.0 - radius;

            if (x > dr)
            {
                x = dr;
            }
            else if (x < -dr)
            {
                x = -dr;
            }

            if (y > dr)
            {
                y = dr;
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

            var dr = 1.0 - radius;
            if (px >= 0)
            {
                if (x >= dr)
                {
                    px = 0;
                }
            }
            else
            {
                if (x <= -dr)
                {
                    px = 0;
                }
            }

            if (py >= 0)
            {
                if (y >= dr)
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

            var dr = 1.0 - radius;
            return x >=  dr 
                || x <= -dr 
                || y >=  dr 
                || y <=  radius;
        }

        public void Recieve(WorldTimeUpdated worldTimeUpdated)
        {
            TimeSeconds = worldTimeUpdated.NewTime;
            ElapsedSeconds = worldTimeUpdated.Elapsed;
        }

        internal World(WorldId id, Vector spawnPosition) : base(id)
        {
            _spawnPosition = spawnPosition;
            TimeSeconds = 0.0;
        }

        private const double GravityAcceleration = 4d;

        private readonly Vector _spawnPosition;
    }
}
