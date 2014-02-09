using System;
using DarkDefenders.Domain.Infrastructure;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Creatures.Events
{
    internal class Fired: EventOf<Creature>
    {
        private readonly TimeSpan _time;

        public Fired(Creature creature, TimeSpan time) : base(creature)
        {
            _time = time;
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Creature> id)
        {
        }

        protected override void Apply(Creature creature)
        {
            creature.SetFireActivationTime(_time);
        }
    }
}