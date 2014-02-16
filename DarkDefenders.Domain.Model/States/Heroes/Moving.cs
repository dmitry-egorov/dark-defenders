using DarkDefenders.Domain.Model.Entities;

namespace DarkDefenders.Domain.Model.States.Heroes
{
    internal class Moving : IHeroState
    {
        private readonly Creature _creature;
        private readonly HeroStateFactory _stateFactory;

        public Moving(HeroStateFactory stateFactory, Creature creature)
        {
            _creature = creature;
            _stateFactory = stateFactory;
        }

        public void Update()
        {
            if (_creature.IsInTheAir())
            {
                var fallenFrom = _creature.GetFallingFrom();

                _stateFactory.Falling(fallenFrom);

                return;
            }

            if (!_creature.IsMovingIntoAWall())
            {
                return;
            }

            if (_creature.CanJumpOver())
            {
                _creature.Jump();

                _stateFactory.Jumping();
            }
            else
            {
                _creature.InvertMovement();
            }
        }
    }
}