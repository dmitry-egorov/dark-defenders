using System.Collections.Generic;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;

namespace DarkDefenders.Domain.Heroes.States
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

        public IEnumerable<IDomainEvent> Update()
        {
            if (_creature.IsInTheAir())
            {
                var fallenFrom = _creature.GetFallingFrom();

                yield return _stateFactory.CreateFallingEvent(fallenFrom);

                yield break;
            }

            if (!_creature.IsMovingIntoAWall())
            {
                yield break;
            }

            if (_creature.CanJumpOver())
            {
                var events = _creature.Jump();

                foreach (var e in events) yield return e;

                yield return _stateFactory.CreateJumpingEvent();
            }
            else
            {
                var stop = _creature.InvertMovement();

                foreach (var e in stop) yield return e;
            }
        }
    }
}