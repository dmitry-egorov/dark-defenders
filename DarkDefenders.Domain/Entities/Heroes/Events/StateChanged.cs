using DarkDefenders.Domain.Entities.Heroes.States;
using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Dtos.Entities.Heroes;
using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Domain.Entities.Heroes.Events
{
    internal class StateChanged : DomainEvent<Hero, HeroId>
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

        protected override IEventDto CreateDto(HeroId id)
        {
            return new StateChangedDto(id, _state.GetDto());
        }
    }
}