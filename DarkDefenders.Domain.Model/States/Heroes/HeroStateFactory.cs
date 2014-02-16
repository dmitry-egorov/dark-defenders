using System;
using System.Collections.Generic;
using System.Drawing;
using DarkDefenders.Domain.Model.Entities;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.States.Heroes
{
    internal class HeroStateFactory
    {
        private readonly Random _random;
        private readonly Creature _creature;
        private readonly Hero _hero;

        public static IHeroState CreateInitial(Random random, Hero hero, Creature creature)
        {
            var factory = new HeroStateFactory(random, hero, creature);

            return factory.CreateDeciding();
        }

        private HeroStateFactory(Random random, Hero hero, Creature creature)
        {
            _random = random;
            _creature = creature;
            _hero = hero;
        }

        public IEnumerable<IEvent> CreateMovingEvent()
        {
            var state = CreateMoving();

            var events = _hero.ChangeState(state);

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IEvent> CreateFallingEvent(Point fallenFrom)
        {
            var state = CreateFalling(fallenFrom);

            var events = _hero.ChangeState(state);

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IEvent> CreateJumpingEvent()
        {
            var state = CreateJumping();

            var events = _hero.ChangeState(state);

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IEvent> CreateDecidingEvent()
        {
            var state = CreateDeciding();

            var events = _hero.ChangeState(state);

            foreach (var e in events) { yield return e; }
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