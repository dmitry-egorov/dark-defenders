using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Other;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Creatures.Events
{
    public class MovementChanged: EventBase<CreatureId, MovementChanged>, ICreatureEvent
    {
        public Movement Movement { get; private set; }

        public MovementChanged(CreatureId rootId, Movement movement) : base(rootId)
        {
            Movement = movement;
        }

        public void ApplyTo(ICreatureEventsReciever reciever)
        {
            reciever.Recieve(this);
        }

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}