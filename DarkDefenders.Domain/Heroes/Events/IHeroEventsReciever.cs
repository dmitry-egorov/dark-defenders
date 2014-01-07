namespace DarkDefenders.Domain.Heroes.Events
{
    public interface IHeroEventsReciever
    {
        void Recieve(StateChanged stateChanged);
    }
}