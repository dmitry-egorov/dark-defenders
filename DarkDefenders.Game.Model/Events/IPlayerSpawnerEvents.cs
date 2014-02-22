using Infrastructure.DDDES;

namespace DarkDefenders.Game.Model.Events
{
    public interface IPlayerSpawnerEvents : IEntityEvents
    {
        void Created(string mapId);
    }
}