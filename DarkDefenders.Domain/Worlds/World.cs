using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DarkDefenders.Domain.Other;
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
        public bool IsTerrainSolidAt(Axis mainAxis, int main, int other)
        {
            return _terrain.At(mainAxis, main, other) == Tile.Solid;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsTerrainSolidAt(int x, int y)
        {
            return _terrain[x, y] == Tile.Solid;
        }

        public void Recieve(WorldTimeUpdated worldTimeUpdated)
        {
            TimeSeconds = worldTimeUpdated.NewTime;
            ElapsedSeconds = worldTimeUpdated.Elapsed;
        }

        internal World(WorldId id, Map<Tile> terrain, Vector spawnPosition) : base(id)
        {
            _terrain = terrain;
            _spawnPosition = spawnPosition;
            TimeSeconds = 0.0;
        }

        
        private readonly Vector _spawnPosition;
        private readonly Map<Tile> _terrain;
    }
}
