using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.States.Heroes;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Events
{
    public interface IHeroEvents : IEntityEvents
    {
        void Created(Creature creature);
        void StateChanged(IHeroState heroState);
    }
}