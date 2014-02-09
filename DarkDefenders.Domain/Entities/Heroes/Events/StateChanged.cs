using DarkDefenders.Domain.Data.Entities.Heroes;
using DarkDefenders.Domain.Entities.Heroes.States;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Heroes.Events
{
    internal class StateChanged : Event<Hero>
    {
        private readonly IHeroState _state;

        public StateChanged(Hero hero, IHeroState state) : base(hero)
        {
            _state = state;
        }

        protected override void Apply(Hero hero)
        {
            hero.SetState(_state);
        }

        protected override object CreateData(IdentityOf<Hero> id)
        {
            return new StateChangedData(id, _state.GetData());
        }
    }
}