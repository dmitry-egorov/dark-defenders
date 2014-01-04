using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Creatures.Events
{
    public class CreatureFired: EventBase<CreatureId, CreatureFired>, ICreatureEvent
    {
        public double Time { get; private set; }

        public CreatureFired(CreatureId rootId, double time) : base(rootId)
        {
            Time = time;
        }

        protected override string ToStringInternal()
        {
            return "Creature fired: {0}, {1}".FormatWith(RootId, Time);
        }

        protected override bool EventEquals(CreatureFired other)
        {
            return Time.Equals(other.Time);
        }

        protected override int GetEventHashCode()
        {
            return Time.GetHashCode();
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