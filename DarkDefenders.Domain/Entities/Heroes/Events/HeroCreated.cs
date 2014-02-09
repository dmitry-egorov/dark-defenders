using System;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Infrastructure;
using Infrastructure.DDDES;

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

        protected override Hero Create()
        {
            return new Hero(_storage, _creature.Item, _random);
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Hero> id)
        {
            reciever.HeroCreated(_creature.Item.Id);
        }
    }
}