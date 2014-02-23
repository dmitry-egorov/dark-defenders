using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Game.Model.Events;
using DarkDefenders.Game.Model.States.Heroes;

namespace DarkDefenders.Remote.AdapterFromGame.Internals.Entities
{
    internal class HeroAdapter : IHeroEvents
    {
        private readonly RemoteEventsAdapter _adapter;

        public HeroAdapter(RemoteEventsAdapter adapter)
        {
            _adapter = adapter;
        }

        public void Created(Creature creature)
        {
            _adapter.HeroCreated(creature);
        }

        public void StateChanged(IHeroState heroState)
        {
        }

        public void Destroyed()
        {
        }
    }
}