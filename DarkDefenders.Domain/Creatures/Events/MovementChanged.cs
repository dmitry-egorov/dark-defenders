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

        protected override string ToStringInternal()
        {
            return "Movement force changed: {0}, {1}".FormatWith(RootId, Movement);
        }

        protected override bool EventEquals(MovementChanged other)
        {
            return Movement.Equals(other.Movement);
        }

        protected override int GetEventHashCode()
        {
            return Movement.GetHashCode();
        }

        public void ApplyTo(ICreatureEventsReciever reciever)
        {
            reciever.Recieve(this);
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}