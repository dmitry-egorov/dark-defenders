using Infrastructure.DDDEventSourcing.Implementations.Domain;

namespace DarkDefenders.Domain.Player.Event
{
    public class Created : EventBase<Id>, IEvent
    {
        public Created(Id rootId) : base(rootId)
        {
        }

        public void ApplyTo(IEventReciever reciever)
        {
            reciever.Apply(this);
        }
    }
}