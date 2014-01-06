using DarkDefenders.Domain.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Clocks
{
    public interface IClockEvent: IRootEvent<IClockEventsReciever>, IDomainEvent
    {
    }
}