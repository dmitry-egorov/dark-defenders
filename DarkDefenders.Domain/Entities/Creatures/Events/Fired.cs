using System;
using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Dtos.Entities.Creatures;
using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Domain.Entities.Creatures.Events
{
    internal class Fired: DomainEvent<Creature, CreatureId>
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

        protected override IEventDto CreateDto(CreatureId id)
        {
            return new FiredDto(id, _time);
        }
    }
}