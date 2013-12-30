namespace DarkDefenders.Domain.Worlds.Events
{
    public interface IWorldEventsReciever
    {
        void Recieve(WorldTimeUpdated worldTimeUpdated);
    }
}