using System.Collections.Generic;
using DarkDefenders.Domain.Terrains.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Terrains
{
    public class Terrain : RootBase<TerrainSnapshot, TerrainId>
    {
        public IEnumerable<ITerrainEvent> Create(TerrainId id, Vector spawnPosition)
        {
            AssertDoesntExist();

            yield return new TerrainCreated(id, spawnPosition);
        }

        public Vector GetSpawnPosition()
        {
            AssertExists();

            return Snapshot.SpawnPosition;
        }

        public bool IsAllowedPosition(Vector position)
        {
            return position.X >= -1.0 && position.X <= 1.0;
        }
    }
}
