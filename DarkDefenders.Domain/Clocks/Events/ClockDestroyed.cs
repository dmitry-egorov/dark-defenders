using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Clocks.Events
{
    public class ClockDestroyed : Destroyed<ClockId, Clock, ClockDestroyed>, IDomainEvent
    {
        public ClockDestroyed(ClockId rootId)
            : base(rootId)
        {
        }

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}