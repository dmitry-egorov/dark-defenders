using System;
using DarkDefenders.Domain.Data.Entities.Creatures;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Creatures.Events
{
    internal class Fired: Event<Creature>
    {
        private readonly TimeSpan _time;

        public Fired(Creature creature, TimeSpan time) : base(creature)
        {
            _time = time;
        }

        protected override void Apply(Creature creature)
        {
            creature.SetFireActivationTime(_time);
        }

        protected override object CreateData(IdentityOf<Creature> id)
        {
            return new FiredData(id, _time);
        }
    }
}