using Infrastructure.DDDES;

namespace DarkDefenders.Game.Model.Events
{
    public interface IWorldEvents : IEntityEvents
    {
        void Created();
    }
}