using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.Heroes.Events;
using DarkDefenders.Dtos.Entities.Creatures;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Entities.Heroes
{
    internal class HeroFactory
    {
        private readonly CreatureFactory _creatureFactory;
        private readonly Random _random;
        private readonly IStorage<Hero> _storage;

        public HeroFactory
        (
            IStorage<Hero> storage,
            CreatureFactory creatureFactory, 
            Random random
        )
        {
            _creatureFactory = creatureFactory;
            _random = random;
            _storage = storage;
        }

        public IEnumerable<IEvent> Create(Vector heroesSpawnPosition, CreatureProperties heroesCreatureProperties)
        {
            var container = new Container<Creature>();

            var events = _creatureFactory.Create(container, heroesSpawnPosition, heroesCreatureProperties);

            return events.ConcatItem(new HeroCreated(_storage, container, _random));
        }
    }
}