using System;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Factories;
using DarkDefenders.Domain.Infrastructure;

using Infrastructure.DDDES;

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

        protected override World Create()
        {
            return new World(_heroFactory, _creatureFactory, _clockContainer.Item, _random, _worldProperties);
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<World> id)
        {
        }
    }
}