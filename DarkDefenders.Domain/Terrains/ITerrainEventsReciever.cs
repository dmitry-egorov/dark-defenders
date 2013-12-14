using DarkDefenders.Domain.Terrains.Events;

namespace DarkDefenders.Domain.Terrains
{
    public interface ITerrainEventsReciever
    {
        void Apply(TerrainCreated terrainCreated);
    }
}