using Infrastructure.DDDEventSourcing.Implementations;

namespace DarkDefenders.Domain.Player.Event
{
    public class Created : EventBase<Id>
    {
        public Created(Id aggregateRootId) : base(aggregateRootId)
        {
        }
    }
}