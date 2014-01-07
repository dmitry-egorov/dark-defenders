using System;
using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Clocks.Events
{
    public class ClockTimeUpdated : EventBase<ClockId, ClockTimeUpdated>, IClockEvent
    {
        public TimeSpan NewTime { get; private set; }

        public ClockTimeUpdated(ClockId clockId, TimeSpan newTime)
            : base(clockId)
        {
            NewTime = newTime;
        }

        public void ApplyTo(IClockEventsReciever reciever)
        {
            reciever.Recieve(this);
        }

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}