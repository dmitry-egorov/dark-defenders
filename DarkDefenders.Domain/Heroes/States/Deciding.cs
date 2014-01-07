using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Other;

namespace DarkDefenders.Domain.Heroes.States
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

        public IEnumerable<IDomainEvent> Update()
        {
            if (_creature.IsInTheAir())
            {
                yield break;
            }

            var bit = _random.Next(2);

            var movement = bit == 0 ? Movement.Left : Movement.Right;

            var events = _creature.SetMovement(movement);

            foreach (var e in events) { yield return e; }

            yield return _factory.CreateMovingEvent();
        }
    }
}