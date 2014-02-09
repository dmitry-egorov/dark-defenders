using DarkDefenders.Domain.Data.Other;
using DarkDefenders.Domain.Infrastructure;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Creatures.Events
{
    internal class MovementChanged : EventOf<Creature>
    {
        private readonly Movement _movement;

        public MovementChanged(Creature creature, Movement movement) : base(creature)
        {
            _movement = movement;
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Creature> id)
        {
        }

        protected override void Apply(Creature creature)
        {
            creature.SetMovement(_movement);
        }
    }
}