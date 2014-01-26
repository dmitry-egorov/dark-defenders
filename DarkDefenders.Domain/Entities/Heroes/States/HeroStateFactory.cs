using System;
using System.Drawing;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.Heroes.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Heroes.States
{
    internal class HeroStateFactory
    {
        private readonly Random _random;
        private readonly Creature _creature;
        private readonly Hero _hero;

        public IHeroState CreateInitial()
        {
            return CreateDeciding();
        }

        public HeroStateFactory(Random random, Hero hero, Creature creature)
        {
            _random = random;
            _creature = creature;
            _hero = hero;
        }

        public IEvent CreateMovingEvent()
        {
            var state = CreateMoving();

            return new StateChanged(_hero, state);
        }

        public IEvent CreateFallingEvent(Point fallenFrom)
        {
            var state = CreateFalling(fallenFrom);

            return new StateChanged(_hero, state);
        }

        public IEvent CreateJumpingEvent()
        {
            var state = CreateJumping();

            return new StateChanged(_hero, state);
        }

        public IEvent CreateDecidingEvent()
        {
            var state = CreateDeciding();

            return new StateChanged(_hero, state);
        }

        private IHeroState CreateDeciding()
        {
            return new Deciding(_random, this, _creature);
        }

        private IHeroState CreateMoving()
        {
            return new Moving(this, _creature);
        }

        private IHeroState CreateJumping()
        {
            return new Jumping(this, _creature);
        }

        private IHeroState CreateFalling(Point fallenFrom)
        {
            return new Falling(this, _creature, fallenFrom);
        }
    }
}