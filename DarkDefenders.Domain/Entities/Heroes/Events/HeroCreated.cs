using System;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Heroes.Events
{
    internal class HeroCreated: Created<Hero>
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

        protected override Hero Create()
        {
            return new Hero(_storage, _creatureContainer.Item, _random);
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Hero> id)
        {
            reciever.HeroCreated(_creatureContainer.Item.Id);
        }
    }
}