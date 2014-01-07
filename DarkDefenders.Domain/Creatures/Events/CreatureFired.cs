using System;
using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Creatures.Events
{
    public class CreatureFired: EventBase<CreatureId, CreatureFired>, ICreatureEvent
    {
        public TimeSpan Time { get; private set; }

        public CreatureFired(CreatureId rootId, TimeSpan time) : base(rootId)
        {
            Time = time;
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