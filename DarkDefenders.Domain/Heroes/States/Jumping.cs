using System.Collections.Generic;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;

namespace DarkDefenders.Domain.Heroes.States
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

        public IEnumerable<IDomainEvent> Update()
        {
            if (!_creature.IsInTheAir())
            {
                yield return _stateFactory.CreateMovingEvent();
            }
        }
    }
}