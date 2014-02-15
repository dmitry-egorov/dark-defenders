using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Creatures.Events
{
    internal class CreatureDestroyed : Destroyed<Creature>
    {
        public CreatureDestroyed(Creature creature, IStorage<Creature> storage) : base(creature, storage)
        {
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Creature> id)
        {
        }
    }
}