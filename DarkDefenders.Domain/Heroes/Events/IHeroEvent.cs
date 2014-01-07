using DarkDefenders.Domain.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Heroes.Events
{
    public interface IHeroEvent: IRootEvent<IHeroEventsReciever>, IDomainEvent
    {
    }
}