using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Data.Entities.Creatures;
using DarkDefenders.Domain.Entities.Heroes;
using DarkDefenders.Domain.Entities.Heroes.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Factories
{
    [UsedImplicitly]
    internal class HeroFactory: Factory<Hero>
    {
        private readonly CreatureFactory _creatureFactory;
        private readonly Random _random;

        public HeroFactory
        (
            IStorage<Hero> storage,
            CreatureFactory creatureFactory, 
            Random random
        ) 
        : base(storage)
        {
            _creatureFactory = creatureFactory;
            _random = random;
        }

        public ICreation<Hero> Create(Vector heroesSpawnPosition, CreatureProperties heroesCreatureProperties)
        {
            return GetCreation(s => YieldCreate(s, heroesSpawnPosition, heroesCreatureProperties));
        }

        private IEnumerable<IEvent> YieldCreate(IStorage<Hero> storage, Vector heroesSpawnPosition, CreatureProperties heroesCreatureProperties)
        {
            var creation = _creatureFactory.Create(heroesSpawnPosition, heroesCreatureProperties);

            foreach (var e in creation) { yield return e; }

            yield return new HeroCreated(storage, creation, _random);
        }
    }
}