using System.Collections.Generic;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Worlds
{
    public class World : RootBase<WorldId, IWorldEventsReciever, IWorldEvent>, IWorldEventsReciever
    {
        public double WorldElapsedSeconds
        {
            get
            {
                AssertExists();
                return _worldElapsedSeconds;
            }
        }

        public IEnumerable<IWorldEvent> Create(Vector spawnPosition)
        {
            AssertDoesntExist();

            yield return new WorldCreated(Id, spawnPosition);
        }

        public IEnumerable<IWorldEvent> UpdateWorldTime(double elapsed)
        {
            AssertExists();
            var newTime = WorldElapsedSeconds + elapsed;
            yield return new WorldTimeUpdated(Id, newTime);
        }

        public Vector GetSpawnPosition()
        {
            AssertExists();
            return _spawnPosition;
        }

        public Vector AdjustCirclePosition(Circle circle)
        {
            AssertExists();

            var position = circle.Position;
            var radius = circle.Radius;
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

        public bool IsInTheAir(Circle boundingCircle)
        {
            AssertExists();
            return boundingCircle.IsAboveHorizontalAxis();
        }

        public Vector GetGravityForce(double mass)
        {
            AssertExists();
            return Vector.XY(0, -mass * GravityAcceleration);
        }

        public Vector ApplyInelasticTerrainImpact(Vector momentum, Circle boundingCircle)
        {
            AssertExists();
            var px = momentum.X;
            var py = momentum.Y;

            var x = boundingCircle.Position.X;
            var y = boundingCircle.Position.Y;
            var radius = boundingCircle.Radius;

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

        public void Apply(WorldCreated worldCreated)
        {
            _spawnPosition = worldCreated.SpawnPosition;
            _worldElapsedSeconds = 0.0;
        }

        public void Apply(WorldTimeUpdated worldTimeUpdated)
        {
            _worldElapsedSeconds = worldTimeUpdated.NewTime;
        }

        internal World(WorldId id) : base(id)
        {
        }

        private const double GravityAcceleration = 4d;

        private Vector _spawnPosition;
        private double _worldElapsedSeconds;
    }
}
