using System;
using System.Drawing;
using DarkDefenders.Game.Model.Entities;

namespace DarkDefenders.Game.Model.States.Heroes
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

        public void Moving()
        {
            var state = CreateMoving();

            _hero.ChangeState(state);
        }

        public void Falling(Point fallenFrom)
        {
            var state = CreateFalling(fallenFrom);

            _hero.ChangeState(state);
        }

        public void Jumping()
        {
            var state = CreateJumping();

            _hero.ChangeState(state);
        }

        public void Deciding()
        {
            var state = CreateDeciding();

            _hero.ChangeState(state);
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