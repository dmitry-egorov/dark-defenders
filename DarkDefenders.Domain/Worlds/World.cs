using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Worlds
{
    public class World : RootBase<WorldId, WorldSnapshot, IWorldEventsReciever, IWorldEvent>
    {
        private const double Friction = 2d;
        private const double GravityAcceleration = 4d;

        internal World(WorldId id) : base(id)
        {
        }

        public IEnumerable<IWorldEvent> Create(Vector spawnPosition)
        {
            AssertDoesntExist();

            yield return new WorldCreated(Id, spawnPosition);
        }

        public Vector GetSpawnPosition()
        {
            var snapshot = Snapshot;

            return snapshot.SpawnPosition;
        }

        public Vector AdjustPosition(Vector position, double radius)
        {
            var x = position.X;
            var y = position.Y;

            if (x + radius > 1.0)
            {
                x = 1.0 - radius;
            }
            else if (x - radius < -1.0)
            {
                x = -1.0 + radius;
            }
            if (y + radius > 1.0)
            {
                y = 1.0 - radius;
            }
            else if (y - radius < 0.0)
            {
                y = radius;
            }

            return Vector.XY(x, y);
        }

        public bool IsInTheAir(Vector position, double radius)
        {
            return position.Y - radius > 0d;
        }

        public Vector GetFrictionForce(Vector momentum, double mass, TimeSpan elapsed)
        {
            var px = momentum.X;

            var sign = -Math.Sign(px);
            var vx = Math.Abs(momentum.X);

            var totalSeconds = elapsed.TotalSeconds;
            var frictionForce = mass * Friction;

            var fx = Math.Min(vx / totalSeconds, frictionForce);

            return Vector.XY(sign * fx, 0);
        }

        public Vector GetGravityForce(double mass)
        {
            return Vector.XY(0, -mass * GravityAcceleration);
        }

        public Vector ApplyInelasticTerrainImpact(Vector position, double radius, Vector momentum)
        {
            var px = momentum.X;
            var py = momentum.Y;

            var x = position.X;
            var y = position.Y;

            if (px >= 0)
            {
                if (x + radius >= 1.0)
                {
                    px = 0;
                }
            }
            else
            {
                if (x - radius <= -1.0)
                {
                    px = 0;
                }
            }

            if (py >= 0)
            {
                if (y + radius >= 1.0)
                {
                    py = 0;
                }
            }
            else
            {
                if (y - radius <= 0.0)
                {
                    py = 0;
                }
            }

            return Vector.XY(px, py);
        }
    }
}
