using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Heroes.Events
{
    public class HeroCreated : EventBase<HeroId, HeroCreated>, IDomainEvent
    {
        public CreatureId CreatureId { get; private set; }

        public HeroCreated(HeroId rootId, CreatureId creatureId)
            : base(rootId)
        {
            CreatureId = creatureId;
        }

        protected override string ToStringInternal()
        {
            return "Hero created: {0}, {1}".FormatWith(RootId, CreatureId);
        }

        protected override bool EventEquals(HeroCreated other)
        {
            return CreatureId.Equals(other.CreatureId);
        }

        protected override int GetEventHashCode()
        {
            return CreatureId.GetHashCode();
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}