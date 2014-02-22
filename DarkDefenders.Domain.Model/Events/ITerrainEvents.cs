using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Events
{
    public interface ITerrainEvents : IEntityEvents
    {
        void Created(string mapId);
    }
}