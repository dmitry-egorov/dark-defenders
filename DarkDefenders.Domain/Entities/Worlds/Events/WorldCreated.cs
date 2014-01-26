using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.Heroes;
using DarkDefenders.Domain.Interfaces;
using DarkDefenders.Dtos.Entities.Creatures;
using DarkDefenders.Dtos.Entities.Worlds;
using DarkDefenders.Dtos.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Entities.Worlds.Events
{
    internal class WorldCreated : Created<World, WorldId>
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

        protected override object CreateDto(WorldId rootId)
        {
            return new WorldCreatedDto(rootId, _worldProperties);
        }

        protected override World Create()
        {
            return new World(_heroFactory, _creatureFactory, _clockContainer, _random, _worldProperties);
        }
    }
}