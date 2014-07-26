using System;
using Infrastructure.DDDES;

namespace DarkDefenders.Game.Model.Events
{
    public interface IClockEvents : IEntityEvents
    {
        void Created();
        void TimeChanged(TimeSpan newTime);
    }
}