using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Creatures.Events
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