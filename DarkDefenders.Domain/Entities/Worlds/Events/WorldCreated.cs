using System;
using DarkDefenders.Domain.Data.Entities.Worlds;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.Heroes;
using DarkDefenders.Domain.Factories;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Worlds.Events
{
    internal class WorldCreated : Created<World>
    {
        private readonly CreatureFactory _creatureFactory;
        private readonly HeroFactory _heroFactory;
        private readonly IContainer<Clock> _clockContainer;
        private readonly Random _random;

        private readonly WorldProperties _worldProperties;

        public WorldCreated
        (
            IStorage<World> storage,
            CreatureFactory creatureFactory,
            HeroFactory heroFactory,
            IContainer<Clock> clockContainer,
            Random random,
            WorldProperties worldProperties
        )
        : base(storage)
        {
            _creatureFactory = creatureFactory;
            _heroFactory = heroFactory;
            _clockContainer = clockContainer;
            _random = random;

            _worldProperties = worldProperties;
        }

        protected override object CreateData(IdentityOf<World> id)
        {
            return new WorldCreatedData(id, _worldProperties);
        }

        protected override World Create()
        {
            return new World(_heroFactory, _creatureFactory, _clockContainer.Item, _random, _worldProperties);
        }
    }
}