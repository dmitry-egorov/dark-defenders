using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Events
{
    public interface IPlayerSpawnerEvents : IEntityEvents
    {
        void Created(string mapId);
    }
}