using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Data.Entities.Worlds;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Worlds;
using DarkDefenders.Domain.Entities.Worlds.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Factories
{
    internal class WorldFactory: Factory<World>
    {
        private readonly CreatureFactory _creatureFactory;
        private readonly HeroFactory _heroFactory;
        private readonly Random _random;
        private readonly IContainer<Clock> _clockContainer;

        public WorldFactory(IStorage<World> storage, CreatureFactory creatureFactory, HeroFactory heroFactory, Random random, IContainer<Clock> clockContainer) 
            : base(storage)
        {
            _creatureFactory = creatureFactory;
            _heroFactory = heroFactory;
            _random = random;
            _clockContainer = clockContainer;
        }

        public ICreation<World> Create(WorldProperties worldProperties)
        {
            return GetCreation(s => YieldEvents(s, worldProperties));
        }

        private IEnumerable<IEvent> YieldEvents(IStorage<World> storage, WorldProperties worldProperties)
        {
            yield return new WorldCreated(storage, _creatureFactory, _heroFactory, _clockContainer, _random, worldProperties);
        }
    }
}