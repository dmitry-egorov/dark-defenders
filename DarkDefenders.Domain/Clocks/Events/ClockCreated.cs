using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Clocks.Events
{
    public class ClockCreated : EventBase<ClockId, ClockCreated>, IDomainEvent
    {
        public ClockCreated(ClockId rootId) : base(rootId)
        {
        }

        protected override string ToStringInternal()
        {
            return "Clock created: {0}".FormatWith(RootId);
        }

        protected override bool EventEquals(ClockCreated other)
        {
            return true;
        }

        protected override int GetEventHashCode()
        {
            return 1;
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}