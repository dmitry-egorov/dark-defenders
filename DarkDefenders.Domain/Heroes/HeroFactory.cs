using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Heroes.Events;
using DarkDefenders.Domain.Heroes.States;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Terrains;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Heroes
{
    public class HeroFactory : RootFactory<HeroId, Hero, HeroCreated>
    {
        private readonly CreatureFactory _creatureFactory;
        private readonly IRepository<CreatureId, Creature> _creatureRepositiory;
        private readonly Random _random;

        public HeroFactory
        (
            IRepository<HeroId, Hero> heroRepository, 
            IRepository<CreatureId, Creature> creatureRepositiory, 
            CreatureFactory creatureFactory, 
            Random random
        )
        : base(heroRepository)
        {
            _creatureRepositiory = creatureRepositiory;
            _creatureFactory = creatureFactory;
            _random = random;
        }

        public IEnumerable<IDomainEvent> Create(HeroId heroId, ClockId clockId, TerrainId terrainId, Vector heroesSpawnPosition, CreatureProperties heroesCreatureProperties)
        {
            AssertDoesntExist(heroId);

            var heroCreatureId = new CreatureId();
            var rigidBodyId = new RigidBodyId();

            var events = _creatureFactory.Create(heroCreatureId, rigidBodyId, clockId, terrainId, heroesSpawnPosition, heroesCreatureProperties);

            foreach (var e in events) { yield return e; }

            yield return new HeroCreated(heroId, terrainId, heroCreatureId);
        }

        protected override Hero Handle(HeroCreated creationEvent)
        {
            var creature = _creatureRepositiory.GetById(creationEvent.CreatureId);
            var stateFactory = new HeroStateFactory(_random, creationEvent.RootId, creature);

            return new Hero(creationEvent.RootId, creature, stateFactory);
        }
    }
}