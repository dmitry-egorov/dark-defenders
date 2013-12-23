using DarkDefenders.Domain.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Players
{
    public interface IPlayerEvent : IRootEvent<PlayerId, IPlayerEventsReciever>, IDomainEvent
    {
    }
}