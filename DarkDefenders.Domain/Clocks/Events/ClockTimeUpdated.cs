using System;
using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

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

        protected override string ToStringInternal()
        {
            return "World time updated: {0}, {1}".FormatWith(RootId, NewTime);
        }

        protected override bool EventEquals(ClockTimeUpdated other)
        {
            return NewTime.Equals(other.NewTime);
        }

        protected override int GetEventHashCode()
        {
            return NewTime.GetHashCode();
        }

        public void ApplyTo(IClockEventsReciever reciever)
        {
            reciever.Recieve(this);
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}