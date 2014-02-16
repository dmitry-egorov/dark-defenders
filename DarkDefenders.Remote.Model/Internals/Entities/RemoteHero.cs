using DarkDefenders.Domain.Model.Entities;
using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Domain.Model.States.Heroes;
using Infrastructure.DDDES;

namespace DarkDefenders.Remote.Model.Internals.Entities
{
    internal class RemoteHero: IHeroEvents
    {
        private readonly RemoteEventAdapter _adapter;

        public RemoteHero(RemoteEventAdapter adapter)
        {
            _adapter = adapter;
        }

        public void Created(IdentityOf<Creature> creatureId)
        {
            _adapter.HeroCreated(creatureId);
        }

        public void StateChanged(IHeroState heroState)
        {
        }

        public void Destroyed()
        {
        }
    }
}