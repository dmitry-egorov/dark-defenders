using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Clocks.Events
{
    public class ClockCreated : EventBase<ClockId, ClockCreated>, IDomainEvent
    {
        public ClockCreated(ClockId rootId) : base(rootId)
        {
        }

        

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}