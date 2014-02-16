using System;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Events
{
    public interface IClockEvents: IEntityEvents
    {
        void Created();
        void TimeChanged(TimeSpan newTime);
    }
}