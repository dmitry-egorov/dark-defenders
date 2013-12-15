using DarkDefenders.Domain.Terrains.Events;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Terrains
{
    public class TerrainSnapshot : ITerrainEventsReciever
    {
        public Vector SpawnPosition { get; private set; }

        public void Apply(TerrainCreated terrainCreated)
        {
            SpawnPosition = terrainCreated.SpawnPosition;
        }
    }
}