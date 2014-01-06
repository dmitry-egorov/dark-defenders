using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Heroes;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain
{
    public class HeroDestroyed : Destroyed<HeroId, Hero, HeroDestroyed>, IDomainEvent
    {
        public HeroDestroyed(HeroId rootId)
            : base(rootId)
        {
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}