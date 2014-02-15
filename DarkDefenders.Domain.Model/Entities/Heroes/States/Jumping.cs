using System.Collections.Generic;
using DarkDefenders.Domain.Model.Entities.Creatures;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Heroes.States
{
    internal class Jumping : IHeroState
    {
        private readonly Creature _creature;
        private readonly HeroStateFactory _stateFactory;

        public Jumping(HeroStateFactory stateFactory, Creature creature)
        {
            _creature = creature;
            _stateFactory = stateFactory;
        }

        public IEnumerable<IEvent> Update()
        {
            if (!_creature.IsInTheAir())
            {
                yield return _stateFactory.CreateMovingEvent();
            }
        }
    }
}