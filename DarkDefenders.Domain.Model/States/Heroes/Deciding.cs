using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.Util;

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

        public IEnumerable<IEvent> Update()
        {
            if (_creature.IsInTheAir())
            {
                yield break;
            }

            var bit = _random.Next(2);

            var movement = bit == 0 ? Movement.Left : Movement.Right;

            var mevents = _creature.ChangeMovementTo(movement);

            var sevents = _factory.CreateMovingEvent();

            var events = Concat.All(mevents, sevents);
            foreach (var e in events) { yield return e; }
        }
    }
}