using System;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Clocks.Events
{
    internal class TimeChanged : EventOf<Clock>
    {
        private readonly TimeSpan _newTime;

        public TimeChanged(Clock clock, TimeSpan newTime) 
            : base(clock)
        {
            _newTime = newTime;
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Clock> id)
        {
        }

        protected override void Apply(Clock clock)
        {
            clock.TimeChanged(_newTime);
        }
    }
}