using DarkDefenders.Domain.Worlds.Events;

namespace DarkDefenders.Domain.Worlds
{
    public interface IWorldEventsReciever
    {
        void Recieve(WorldTimeUpdated worldTimeUpdated);
    }
}