using System;
using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.Clocks
{
    public class TimeChangedDto : EventDto<TimeChangedDto>
    {
        public ClockId ClockId { get; private set; }
        public TimeSpan NewTime { get; private set; }

        public TimeChangedDto(ClockId clockId, TimeSpan newTime)
        {
            ClockId = clockId;
            NewTime = newTime;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}