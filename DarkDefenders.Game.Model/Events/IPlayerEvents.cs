using DarkDefenders.Game.Model.Entities;
using Infrastructure.DDDES;

namespace DarkDefenders.Game.Model.Events
{
    public interface IPlayerEvents : IEntityEvents
    {
        void Created(Creature creature);
    }
}