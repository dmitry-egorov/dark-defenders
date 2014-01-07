using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Creatures.Events
{
    public class CreatureDestroyed: Destroyed<CreatureId, Creature, CreatureDestroyed>, IDomainEvent
    {
        public CreatureDestroyed(CreatureId rootId) : base(rootId)
        {
        }

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}