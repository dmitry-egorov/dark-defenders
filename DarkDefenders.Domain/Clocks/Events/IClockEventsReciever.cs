namespace DarkDefenders.Domain.Clocks.Events
{
    public interface IClockEventsReciever
    {
        void Recieve(ClockTimeUpdated clockTimeUpdated);
    }
}