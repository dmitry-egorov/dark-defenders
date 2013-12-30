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
        public bool IsInTheAir(Circle circle)
        {
            return _terrain.IsInTheAir(circle);
        }

        public Vector LimitPosition(Circle circle)
        {
            return _terrain.LimitPosition(circle);
        }

        public Vector LimitMomentum(Vector momentum, Circle boundingCircle)
        {
            return _terrain.LimitMomentum(momentum, boundingCircle);
        }

        public bool IsAdjacentToAWall(Circle circle)
        {
            return _terrain.IsAdjacentToAWall(circle);
        }

        public void Recieve(WorldTimeUpdated worldTimeUpdated)
        {
            TimeSeconds = worldTimeUpdated.NewTime;
            ElapsedSeconds = worldTimeUpdated.Elapsed;
        }

        internal World(WorldId id, Terrain terrain, Vector spawnPosition) : base(id)
        {
            _terrain = terrain;
            _spawnPosition = spawnPosition;
            TimeSeconds = 0.0;
        }

        
        private readonly Vector _spawnPosition;
        private readonly Terrain _terrain;
    }
}
