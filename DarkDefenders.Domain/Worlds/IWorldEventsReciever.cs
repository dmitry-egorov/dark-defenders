using DarkDefenders.Domain.Worlds.Events;

namespace DarkDefenders.Domain.Worlds
{
    public interface IWorldEventsReciever
    {
        void Apply(WorldCreated worldCreated);
        void Apply(WorldTimeUpdated worldTimeUpdated);
    }
}