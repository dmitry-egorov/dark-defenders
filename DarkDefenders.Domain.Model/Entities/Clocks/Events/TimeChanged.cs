using System;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Clocks.Events
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