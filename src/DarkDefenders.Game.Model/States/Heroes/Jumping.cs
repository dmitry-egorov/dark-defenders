using DarkDefenders.Game.Model.Entities;

namespace DarkDefenders.Game.Model.States.Heroes
{
    internal class Jumping : IHeroState
    {
        private readonly HeroStateFactory _stateFactory;
        private readonly RigidBody _rigidBody;

        public Jumping(HeroStateFactory stateFactory, RigidBody rigidBody)
        {
            _rigidBody = rigidBody;
            _stateFactory = stateFactory;
        }

        public void Update()
        {
            if (_rigidBody.IsInTheAir())
            {
                return;
            }

            _stateFactory.Moving();
        }
    }
}