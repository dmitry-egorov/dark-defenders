using System.Collections.Generic;
using System.Drawing;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;

namespace DarkDefenders.Domain.Heroes.States
{
    internal class Falling : IHeroState
    {
        private readonly Creature _creature;
        private readonly HeroStateFactory _factory;
        private readonly Point _fallenFrom;

        public Falling(HeroStateFactory factory, Creature creature, Point fallenFrom)
        {
            _creature = creature;
            _fallenFrom = fallenFrom;
            _factory = factory;
        }

        public IEnumerable<IDomainEvent> Update()
        {
            if (_creature.IsInTheAir())
            {
                yield break;
            }

            if (_creature.CanMoveBackwardsAfterFall(_fallenFrom))
            {
                yield return _factory.CreateDecidingEvent();
            }
            else
            {
                yield return _factory.CreateMovingEvent();
            }
        }
    }
}