using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Dtos.Entities.Creatures;
using DarkDefenders.Dtos.Infrastructure;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Creatures.Events
{
    internal class CreatureDestroyed : Destroyed<Creature, CreatureId>
    {
        public CreatureDestroyed(Creature root, IStorage<Creature> storage) : base(root, storage)
        {
        }

        protected override IEventDto CreateDto(CreatureId id)
        {
            return new CreatureDestroyedDto(id);
        }
    }
}