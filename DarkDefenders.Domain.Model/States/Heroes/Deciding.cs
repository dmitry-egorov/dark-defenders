using System;
using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Other;

namespace DarkDefenders.Domain.Model.States.Heroes
{
    internal class Deciding : IHeroState
    {
        private readonly Random _random;
        private readonly HeroStateFactory _factory;
        private readonly Creature _creature;

        public Deciding(Random random, HeroStateFactory factory, Creature creature)
        {
            _random = random;
            _factory = factory;
            _creature = creature;
        }

        public void Update()
        {
            if (_creature.IsInTheAir())
            {
                return;
            }

            var bit = _random.Next(2);

            var movement = bit == 0 ? Movement.Left : Movement.Right;

            _creature.ChangeMovementTo(movement);

            _factory.Moving();
        }
    }
}