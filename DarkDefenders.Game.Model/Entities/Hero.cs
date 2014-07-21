using System;
using DarkDefenders.Game.Model.Events;
using DarkDefenders.Game.Model.States.Heroes;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using JetBrains.Annotations;

namespace DarkDefenders.Game.Model.Entities
{
    [UsedImplicitly]
    public class Hero : Entity<Hero, IHeroEvents>, IHeroEvents
    {
        const string PropertiesId = "Hero";

        private readonly RigidBody _rigidBody;
        private readonly Creature _creature;

        private IHeroState _state;

        public Hero(Random random, Creature creature, RigidBody rigidBody, Terrain terrain)
        {
            _creature = creature;
            _rigidBody = rigidBody;

            _state = HeroStateFactory.CreateInitial(random, this, rigidBody, creature, terrain);
        }

        public void Create(Vector initialPosition)
        {
            _rigidBody.Create(initialPosition, Momentum.Zero, PropertiesId);
            _creature.Create(_rigidBody, PropertiesId);

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