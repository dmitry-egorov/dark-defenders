using DarkDefenders.Domain.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Worlds
{
    public interface IWorldEvent : IRootEvent<WorldId, IWorldEventsReciever>, IDomainEvent
    {
    }
}