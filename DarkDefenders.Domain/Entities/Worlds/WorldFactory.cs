using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.Heroes;
using DarkDefenders.Domain.Entities.Worlds.Events;
using DarkDefenders.Domain.Interfaces;
using DarkDefenders.Dtos.Entities.Worlds;
using Infrastructure.DDDES;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Entities.Worlds
{
    internal class WorldFactory
    {
        private readonly CreatureFactory _creatureFactory;
        private readonly HeroFactory _heroFactory;
        private readonly Random _random;
        private readonly IStorage<World> _storage;
        private readonly IContainer<Clock> _clockContainer;

        public WorldFactory(IStorage<World> storage, CreatureFactory creatureFactory, HeroFactory heroFactory, Random random, IContainer<Clock> clockContainer)
        {
            _creatureFactory = creatureFactory;
            _heroFactory = heroFactory;
            _random = random;
            _clockContainer = clockContainer;
            _storage = storage;
        }

        public IEnumerable<IEvent> Create(WorldProperties worldProperties)
        {
            return new WorldCreated(_storage, _creatureFactory, _heroFactory, _clockContainer, _random, worldProperties).EnumerateOnce();
        }
    }
}