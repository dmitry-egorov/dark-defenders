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
        public bool IsInTheAir(Box box)
        {
            return _terrain.IsInTheAir(box);
        }

        public Vector ApplyPositionChange(Box box, Vector positionChange)
        {
            return _terrain.ApplyPositionChange(box, positionChange);
        }

        public Vector LimitMomentum(Vector momentum, Box boundingBox)
        {
            return _terrain.LimitMomentum(momentum, boundingBox);
        }

        public bool IsAdjacentToAWall(Box box)
        {
            return _terrain.IsAdjacentToAWall(box);
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
