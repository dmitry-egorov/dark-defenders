using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Domain.Model.States.Heroes;

namespace DarkDefenders.Remote.Model.Internals.Entities
{
    internal class RemoteHero: IHeroEvents
    {
        private readonly RemoteEventAdapter _adapter;

        public RemoteHero(RemoteEventAdapter adapter)
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