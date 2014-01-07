using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Heroes.Events
{
    public class HeroDestroyed : Destroyed<HeroId, Hero, HeroDestroyed>, IDomainEvent
    {
        public HeroDestroyed(HeroId rootId)
            : base(rootId)
        {
        }

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}