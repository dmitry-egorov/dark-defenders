using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Dtos.Entities.Creatures;
using DarkDefenders.Dtos.Infrastructure;
using DarkDefenders.Dtos.Other;

namespace DarkDefenders.Domain.Entities.Creatures.Events
{
    internal class MovementChanged : DomainEvent<Creature, CreatureId>
    {
        private readonly Movement _movement;

        public MovementChanged(Creature creature, Movement movement) : base(creature)
        {
            _movement = movement;
        }

        protected override void Apply(Creature creature)
        {
            creature.SetMovement(_movement);
        }

        protected override IEventDto CreateDto(CreatureId id)
        {
            return new MovementChangedDto(id, _movement);
        }
    }
}