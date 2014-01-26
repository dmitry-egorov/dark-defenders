using System;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Dtos.Entities.Heroes;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Heroes.Events
{
    internal class HeroCreated: Created<Hero, HeroId>
    {
        private readonly IStorage<Hero> _storage;
        private readonly Random _random;
        private readonly IContainer<Creature> _creatureContainer;

        public HeroCreated(IStorage<Hero> storage, IContainer<Creature> creatureContainer, Random random) : base(storage)
        {
            _storage = storage;
            _random = random;
            _creatureContainer = creatureContainer;
        }


        protected override object CreateDto(HeroId heroId)
        {
            var creatureId = _creatureContainer.Item.GetGlobalId();

            return new HeroCreatedDto(heroId, creatureId);
        }

        protected override Hero Create()
        {
            var creature = _creatureContainer.Item;

            return new Hero(_storage, creature, _random);
        }
    }
}