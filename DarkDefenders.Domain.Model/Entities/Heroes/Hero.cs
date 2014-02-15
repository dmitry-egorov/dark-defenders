using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Model.Entities.Creatures;
using DarkDefenders.Domain.Model.Entities.Heroes.Events;
using DarkDefenders.Domain.Model.Entities.Heroes.States;
using DarkDefenders.Domain.Model.Entities.RigidBodies;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities.Heroes
{
    [UsedImplicitly]
    public class Hero : Entity<Hero>
    {
        private static readonly RigidBodyProperties _rigidBodyProperties = new RigidBodyProperties(0.4f, 1.0f, 20.0f);
        private static readonly CreatureProperties _creatureProperties = new CreatureProperties(180, 30, _rigidBodyProperties);

        private readonly IStorage<Hero> _storage;
        private readonly Random _random;
        private readonly Creature _creature;

        private IHeroState _state;

        public Hero(IStorage<Hero> storage, Random random, Creature creature)
        {
            _storage = storage;
            _random = random;
            _creature = creature;
        }

        public IEnumerable<IEvent> Create(Vector initialPosition)
        {
            var events = _creature.Create(initialPosition, _creatureProperties);

            foreach (var e in events) { yield return e; }

            yield return new HeroCreated(_storage, this, _creature);
        }

        public IEnumerable<IEvent> Think()
        {
            var events = _state.Update();

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IEvent> Kill()
        {
            yield return new HeroDestroyed(this, _storage);

            var events = _creature.Kill();

            foreach (var e in events) { yield return e; }
        }

        internal void Created(Creature creature)
        {
            _state = HeroStateFactory.CreateInitial(_random, this, _creature);
        }

        internal void StateChanged(IHeroState heroState)
        {
            _state = heroState;
        }
    }
}