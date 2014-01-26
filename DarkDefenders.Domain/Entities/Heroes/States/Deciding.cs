using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.Other;
using DarkDefenders.Dtos;
using DarkDefenders.Dtos.Other;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Heroes.States
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

            var events = _creature.ChangeMovement(movement);

            foreach (var e in events) { yield return e; }

            yield return _factory.CreateMovingEvent();
        }

        public HeroStateDto GetDto()
        {
            return HeroStateDto.Deciding;
        }
    }
}