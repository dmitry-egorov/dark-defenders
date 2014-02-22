using Infrastructure.DDDES;

namespace DarkDefenders.Game.Model.Events
{
    public interface ITerrainEvents : IEntityEvents
    {
        void Created(string mapId);
    }
}