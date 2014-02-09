using System;
using DarkDefenders.Domain.Data.Entities.Heroes;
using DarkDefenders.Domain.Entities.Creatures;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Heroes.Events
{
    internal class HeroCreated: Created<Hero>
    {
        private readonly IStorage<Hero> _storage;
        private readonly Random _random;
        private readonly IContainer<Creature> _creature;

        public HeroCreated(IStorage<Hero> storage, IContainer<Creature> creature, Random random) : base(storage)
        {
            _storage = storage;
            _random = random;
            _creature = creature;
        }

        protected override object CreateData(IdentityOf<Hero> heroId)
        {
            return new HeroCreatedData(heroId, _creature.Item.Id);
        }

        protected override Hero Create()
        {
            return new Hero(_storage, _creature.Item, _random);
        }
    }
}