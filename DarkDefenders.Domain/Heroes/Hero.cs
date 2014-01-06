using System.Collections.Generic;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Heroes.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Heroes
{
    public class Hero: RootBase<HeroId, IHeroEventsReciever, IHeroEvent>, IHeroEventsReciever
    {
        public Hero(HeroId id) : base(id)
        {
        }

        public IEnumerable<IDomainEvent> Think()
        {
            yield break;
        }
    }
}