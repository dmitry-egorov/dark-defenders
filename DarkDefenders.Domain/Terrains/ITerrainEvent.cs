using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Terrains
{
    public interface ITerrainEvent : IRootEvent<TerrainId, ITerrainEventsReciever>
    {
    }
}