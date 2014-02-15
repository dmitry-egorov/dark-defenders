using System;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Weapons.Events
{
    internal class Fired: EventOf<Weapon>
    {
        private readonly TimeSpan _time;

        public Fired(Weapon creature, TimeSpan time)
            : base(creature)
        {
            _time = time;
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Weapon> id)
        {
        }

        protected override void Apply(Weapon creature)
        {
            creature.Fired(_time);
        }
    }
}