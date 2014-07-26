using System;
using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Game.Model.Other;

namespace DarkDefenders.Game.Model.States.Heroes
{
    internal class Deciding : IHeroState
    {
        private readonly Random _random;
        private readonly HeroStateFactory _factory;
        private readonly Creature _creature;
        private readonly RigidBody _rigidBody;

        public Deciding(Random random, HeroStateFactory factory, Creature creature, RigidBody rigidBody)
        {
            _random = random;
            _factory = factory;
            _creature = creature;
            _rigidBody = rigidBody;
        }

        public void Update()
        {
            if (_rigidBody.IsInTheAir())
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