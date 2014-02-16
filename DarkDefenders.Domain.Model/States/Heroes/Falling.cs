using System.Collections.Generic;
using System.Drawing;
using DarkDefenders.Domain.Model.Entities;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.States.Heroes
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

        public IEnumerable<IEvent> Update()
        {
            if (_creature.IsInTheAir())
            {
                yield break;
            }

            if (_creature.CanMoveBackwardsAfterFall(_fallenFrom))
            {
                var events = _factory.CreateDecidingEvent();
                foreach (var e in events) { yield return e; }
            }
            else
            {
                var events = _factory.CreateMovingEvent();
                foreach (var e in events) { yield return e; }
            }
        }
    }
}