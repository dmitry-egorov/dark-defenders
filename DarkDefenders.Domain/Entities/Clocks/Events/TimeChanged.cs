using System;
using DarkDefenders.Domain.Infrastructure;
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
            clock.SetNewTime(_newTime);
        }
    }
}