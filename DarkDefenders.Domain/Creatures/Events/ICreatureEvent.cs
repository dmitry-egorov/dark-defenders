using DarkDefenders.Domain.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Creatures.Events
{
    public interface ICreatureEvent : IRootEvent<ICreatureEventsReciever>, IDomainEvent
    {
    }
}