using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Interfaces;
using DarkDefenders.Domain.Other;
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
            creature.MovementChanged(_movement);
        }
    }
}