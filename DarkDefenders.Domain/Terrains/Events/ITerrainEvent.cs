using DarkDefenders.Domain.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Terrains.Events
{
    public interface ITerrainEvent: IRootEvent<ITerrainEventsReciever>, IDomainEvent
    {
    }
}