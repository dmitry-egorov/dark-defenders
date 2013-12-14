namespace DarkDefenders.Domain.Player.Event
{
    public interface IEventReciever
    {
        void Apply(Created created);
    }
}