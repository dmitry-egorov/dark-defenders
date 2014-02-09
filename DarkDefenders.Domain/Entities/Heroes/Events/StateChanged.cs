using DarkDefenders.Domain.Entities.Heroes.States;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Heroes.Events
{
    internal class StateChanged : EventOf<Hero>
    {
        private readonly IHeroState _state;

        public StateChanged(Hero hero, IHeroState state) : base(hero)
        {
            _state = state;
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Hero> id)
        {
        }

        protected override void Apply(Hero hero)
        {
            hero.StateChanged(_state);
        }
    }
}