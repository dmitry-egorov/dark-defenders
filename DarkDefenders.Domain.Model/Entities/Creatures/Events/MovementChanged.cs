using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Creatures.Events
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