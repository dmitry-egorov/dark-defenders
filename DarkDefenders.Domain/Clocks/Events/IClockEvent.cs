using DarkDefenders.Domain.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Clocks.Events
{
    public interface IClockEvent: IRootEvent<IClockEventsReciever>, IDomainEvent
    {
    }
}