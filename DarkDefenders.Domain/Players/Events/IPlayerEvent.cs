using DarkDefenders.Domain.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Players.Events
{
    public interface IPlayerEvent : IRootEvent<IPlayerEventsReciever>, IDomainEvent
    {
    }
}