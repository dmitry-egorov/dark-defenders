using DarkDefenders.Domain.Data.Entities.Creatures;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Creatures.Events
{
    internal class CreatureDestroyed : Destroyed<Creature>
    {
        public CreatureDestroyed(Creature root, IStorage<Creature> storage) : base(root, storage)
        {
        }

        protected override object CreateData(IdentityOf<Creature> id)
        {
            return new CreatureDestroyedData(id);
        }
    }
}