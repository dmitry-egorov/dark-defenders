using System.Drawing;
using DarkDefenders.Domain.Model.Entities;

namespace DarkDefenders.Domain.Model.States.Heroes
{
    internal class Falling : IHeroState
    {
        private readonly Creature _creature;
        private readonly HeroStateFactory _factory;
        private readonly Point _fallenFrom;

        public Falling(HeroStateFactory factory, Creature creature, Point fallenFrom)
        {
            _creature = creature;
            _fallenFrom = fallenFrom;
            _factory = factory;
        }

        public void Update()
        {
            if (_creature.IsInTheAir())
            {
                return;
            }

            if (_creature.CanMoveBackwardsAfterFall(_fallenFrom))
            {
                _factory.Deciding();
            }
            else
            {
                _factory.Moving();
            }
        }
    }
}