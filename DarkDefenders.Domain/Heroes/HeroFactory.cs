using System.Collections.Generic;
using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Heroes.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Heroes
{
    public class HeroFactory : RootFactory<HeroId, Hero, HeroCreated>
    {
        private readonly CreatureFactory _creatureFactory;

        public HeroFactory(IRepository<HeroId, Hero> heroRepository, CreatureFactory creatureFactory) : base(heroRepository)
        {
            _creatureFactory = creatureFactory;
        }

        public IEnumerable<IDomainEvent> Create(HeroId heroId, ClockId clockId, WorldId worldId, Vector heroesSpawnPosition, CreatureProperties heroesCreatureProperties)
        {
            AssertDoesntExist(heroId);

            var heroCreatureId = new CreatureId();

            var events = _creatureFactory.Create(heroCreatureId, clockId, worldId, heroesSpawnPosition, heroesCreatureProperties);

            foreach (var e in events) { yield return e; }

            yield return new HeroCreated(heroId, heroCreatureId);
        }

        protected override Hero Handle(HeroCreated creationEvent)
        {
            return new Hero(creationEvent.RootId);
        }
    }
}