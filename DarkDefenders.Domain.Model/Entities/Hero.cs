using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Domain.Model.States.Heroes;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities
{
    [UsedImplicitly]
    public class Hero : Entity<Hero, IHeroEvents>, IHeroEvents
    {
        private readonly Creature _creature;

        private IHeroState _state;

        public Hero(IHeroEvents external, IStorage<Hero> storage, Random random, Creature creature)
            : base(external, storage)
        {
            _creature = creature;

            _state = HeroStateFactory.CreateInitial(random, this, _creature);
        }

        public IEnumerable<IEvent> Create(Vector initialPosition)
        {
            var events = _creature.Create(initialPosition, "Hero");

            foreach (var e in events) { yield return e; }

            yield return CreationEvent(x => x.Created(_creature.Id));
        }

        public IEnumerable<IEvent> Think()
        {
            var events = _state.Update();

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IEvent> Kill()
        {
            yield return DestructionEvent();

            var events = _creature.Kill();

            foreach (var e in events) { yield return e; }
        }

        internal IEnumerable<IEvent> ChangeState(IHeroState state)
        {
            yield return Event(x => x.StateChanged(state));
        }

        void IHeroEvents.Created(IdentityOf<Creature> creatureId)
        {
        }

        void IHeroEvents.StateChanged(IHeroState heroState)
        {
            _state = heroState;
        }

        void IEntityEvents.Destroyed()
        {
        }
    }
}