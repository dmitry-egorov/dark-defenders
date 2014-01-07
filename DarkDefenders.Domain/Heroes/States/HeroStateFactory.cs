using System;
using System.Drawing;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;

namespace DarkDefenders.Domain.Heroes.States
{
    internal class HeroStateFactory
    {
        private readonly Random _random;
        private readonly Creature _creature;
        private readonly HeroId _heroId;

        public IHeroState CreateInitial()
        {
            return CreateDeciding();
        }

        public HeroStateFactory(Random random, HeroId heroId, Creature creature)
        {
            _random = random;
            _creature = creature;
            _heroId = heroId;
        }

        public IDomainEvent CreateMovingEvent()
        {
            var state = CreateMoving();

            return new StateChanged(_heroId, state);
        }

        public IDomainEvent CreateFallingEvent(Point fallenFrom)
        {
            var state = CreateFalling(fallenFrom);

            return new StateChanged(_heroId, state);
        }

        public IDomainEvent CreateJumpingEvent()
        {
            var state = CreateJumping();

            return new StateChanged(_heroId, state);
        }

        public IDomainEvent CreateDecidingEvent()
        {
            return new StateChanged(_heroId, CreateDeciding());
        }

        private IHeroState CreateDeciding()
        {
            return new Deciding(_random, this, _creature);
        }

        private IHeroState CreateMoving()
        {
            return new Moving(this, _creature);
        }

        private IHeroState CreateJumping()
        {
            return new Jumping(this, _creature);
        }

        private IHeroState CreateFalling(Point fallenFrom)
        {
            return new Falling(this, _creature, fallenFrom);
        }

        
    }
}