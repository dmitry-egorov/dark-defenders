using System;
using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Dtos.Entities.Clocks;
using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Domain.Entities.Clocks.Events
{
    internal class TimeChanged : DomainEvent<Clock, ClockId>
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

        protected override IEventDto CreateDto(ClockId id)
        {
            return new TimeChangedDto(id, _newTime);
        }
    }
}