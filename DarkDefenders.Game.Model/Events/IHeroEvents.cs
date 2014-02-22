using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Game.Model.States.Heroes;
using Infrastructure.DDDES;

namespace DarkDefenders.Game.Model.Events
{
    public interface IHeroEvents : IEntityEvents
    {
        void Created(Creature creature);
        void StateChanged(IHeroState heroState);
    }
}