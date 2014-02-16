using System.Collections.Generic;
using DarkDefenders.Domain.Model.Entities;
using Infrastructure.DDDES;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Model.States.Heroes
{
    internal class Moving : IHeroState
    {
        private readonly Creature _creature;
        private readonly HeroStateFactory _stateFactory;

        public Moving(HeroStateFactory stateFactory, Creature creature)
        {
            _creature = creature;
            _stateFactory = stateFactory;
        }

        public IEnumerable<IEvent> Update()
        {
            if (_creature.IsInTheAir())
            {
                var fallenFrom = _creature.GetFallingFrom();

                var events = _stateFactory.CreateFallingEvent(fallenFrom);
                foreach (var e in events) { yield return e; }

                yield break;
            }

            if (!_creature.IsMovingIntoAWall())
            {
                yield break;
            }

            if (_creature.CanJumpOver())
            {
                var jevents = _creature.Jump();

                var sevents = _stateFactory.CreateJumpingEvent();

                var events = Concat.All(jevents, sevents);

                foreach (var e in events) { yield return e; }
            }
            else
            {
                var stop = _creature.InvertMovement();

                foreach (var e in stop) yield return e;
            }
        }
    }
}