using DarkDefenders.Domain.Data.Entities.Creatures;
using DarkDefenders.Domain.Data.Other;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Creatures.Events
{
    internal class MovementChanged : Event<Creature>
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

        protected override object CreateData(IdentityOf<Creature> id)
        {
            return new MovementChangedData(id, _movement);
        }
    }
}