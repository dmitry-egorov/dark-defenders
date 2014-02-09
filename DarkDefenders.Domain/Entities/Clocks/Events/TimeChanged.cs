using System;
using DarkDefenders.Domain.Data.Entities.Clocks;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Clocks.Events
{
    internal class TimeChanged : Event<Clock>
    {
        private readonly TimeSpan _newTime;

        public TimeChanged(Clock clock, TimeSpan newTime) 
            : base(clock)
        {
            _newTime = newTime;
        }

        protected override void Apply(Clock clock)
        {
            clock.SetNewTime(_newTime);
        }

        protected override object CreateData(IdentityOf<Clock> id)
        {
            return new TimeChangedData(id, _newTime);
        }
    }
}