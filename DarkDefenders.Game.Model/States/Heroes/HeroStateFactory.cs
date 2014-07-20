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
        private readonly RigidBody _rigidBody;
        private readonly Terrain _terrain;

        public static IHeroState CreateInitial(Random random, Hero hero, RigidBody rigidBody, Creature creature, Terrain terrain)
        {
            var factory = new HeroStateFactory(random, hero, rigidBody, creature, terrain);

            return factory.CreateDeciding();
        }

        private HeroStateFactory(Random random, Hero hero, RigidBody rigidBody, Creature creature, Terrain terrain)
        {
            _random = random;
            _creature = creature;
            _terrain = terrain;
            _hero = hero;
            _rigidBody = rigidBody;
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
            return new Deciding(_random, this, _creature, _rigidBody);
        }

        private IHeroState CreateMoving()
        {
            return new Moving(this, _rigidBody, _creature);
        }

        private IHeroState CreateJumping()
        {
            return new Jumping(this, _rigidBody);
        }

        private IHeroState CreateFalling(Point fallenFrom)
        {
            return new Falling(this, _rigidBody, _creature, fallenFrom, _terrain);
        }
    }
}