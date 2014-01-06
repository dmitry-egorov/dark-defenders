using DarkDefenders.Domain.Clocks.Events;
using DarkDefenders.Domain.Worlds.Events;

namespace DarkDefenders.Domain.Clocks
{
    public interface IClockEventsReciever
    {
        void Recieve(ClockTimeUpdated clockTimeUpdated);
    }
}