using DarkDefenders.Game.Model.Entities;

namespace DarkDefenders.Game.Model.States.Heroes
{
    internal class Jumping : IHeroState
    {
        private readonly Creature _creature;
        private readonly HeroStateFactory _stateFactory;

        public Jumping(HeroStateFactory stateFactory, Creature creature)
        {
            _creature = creature;
            _stateFactory = stateFactory;
        }

        public void Update()
        {
            if (_creature.IsInTheAir())
            {
                return;
            }

            _stateFactory.Moving();
        }
    }
}