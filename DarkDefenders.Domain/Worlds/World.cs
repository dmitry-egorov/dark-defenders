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
            return !IsOnTheGround(position, radius);
        }

        public bool IsOnTheGround(Vector position, double radius)
        {
            return position.Y - radius <= 0d;
        }

        public Vector ApplyFrictionForce(Vector position, double radius, Vector momentum, double mass, TimeSpan elapsed)
        {
            if (IsInTheAir(position, radius))
            {
                return momentum;
            }

            var px = momentum.X;
            var py = momentum.Y;
            var frictionForce = mass * Friction * elapsed.TotalSeconds;
            if (Math.Abs(px) < frictionForce)
            {
                px = 0;
            }
            else if (px > 0)
            {
                px -= frictionForce;
            }
            else if (px < 0)
            {
                px += frictionForce;
            }

            return Vector.XY(px, py);
        }

        public Vector ApplyGravityForce(Vector position, double radius, Vector momentum, double mass, TimeSpan elapsed)
        {
            return IsInTheAir(position, radius) 
                   ? Vector.XY(momentum.X, momentum.Y - mass * GravityAcceleration * elapsed.TotalSeconds) 
                   : momentum;
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
