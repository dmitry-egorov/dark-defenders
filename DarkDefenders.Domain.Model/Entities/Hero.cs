using System;
using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Domain.Model.States.Heroes;
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

        public Hero(Random random, Creature creature)
        {
            _creature = creature;

            _state = HeroStateFactory.CreateInitial(random, this, _creature);
        }

        public void Create(Vector initialPosition)
        {
            _creature.Create(initialPosition, "Hero");

            CreationEvent(x => x.Created(_creature));
        }

        public void Think()
        {
            _state.Update();
        }

        public void Kill()
        {
            DestructionEvent();

            _creature.Kill();
        }

        internal void ChangeState(IHeroState state)
        {
            Event(x => x.StateChanged(state));
        }

        void IHeroEvents.Created(Creature creature)
        {
        }

        void IHeroEvents.StateChanged(IHeroState heroState)
        {
            _state = heroState;
        }
    }
}