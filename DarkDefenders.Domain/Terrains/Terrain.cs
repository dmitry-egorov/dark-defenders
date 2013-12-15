using System.Collections.Generic;
using DarkDefenders.Domain.Terrains.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Terrains
{
    public class Terrain : RootBase<TerrainId, TerrainSnapshot, ITerrainEventsReciever, ITerrainEvent>
    {
        public Terrain(TerrainId id) : base(id)
        {
        }

        public IEnumerable<ITerrainEvent> Create(Vector spawnPosition)
        {
            AssertDoesntExist();

            yield return new TerrainCreated(Id, spawnPosition);
        }

        public Vector GetSpawnPosition()
        {
            var snapshot = Snapshot;

            return snapshot.SpawnPosition;
        }

        public bool IsAllowedPosition(Vector position)
        {
            return position.X >= -1.0 && position.X <= 1.0;
        }
    }
}
