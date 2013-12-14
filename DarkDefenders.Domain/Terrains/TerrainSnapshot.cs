using DarkDefenders.Domain.Terrains.Events;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Terrains
{
    public class TerrainSnapshot : ITerrainEventsReciever, IRootSnapshot<TerrainId>
    {
        public TerrainId Id { get; private set; }
        public Vector SpawnPosition { get; private set; }

        public void Apply(TerrainCreated terrainCreated)
        {
            Id = terrainCreated.RootId;
            SpawnPosition = terrainCreated.SpawnPosition;
        }
    }
}