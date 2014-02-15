using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.Heroes.Events;
using DarkDefenders.Domain.Entities.Heroes.States;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Heroes
{
    public class Hero : Entity<Hero>
    {
        private readonly IStorage<Hero> _storage;
        private readonly Creature _creature;

        private IHeroState _state;

        internal Hero(IStorage<Hero> storage, Creature creature, Random random)
        {
            _storage = storage;
            _creature = creature;

            var stateFactory = new HeroStateFactory(random, this, creature);
            _state = stateFactory.CreateInitial();
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

        internal void StateChanged(IHeroState heroState)
        {
            _state = heroState;
        }
    }
}