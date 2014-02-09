using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Creatures.Events
{
    internal class CreatureDestroyed : Destroyed<Creature>
    {
        public CreatureDestroyed(Creature root, IStorage<Creature> storage) : base(root, storage)
        {
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Creature> id)
        {
        }
    }
}