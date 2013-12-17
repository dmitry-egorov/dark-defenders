using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Worlds
{
    public class WorldSnapshot : IWorldEventsReciever
    {
        public Vector SpawnPosition { get; private set; }

        public void Apply(WorldCreated worldCreated)
        {
            SpawnPosition = worldCreated.SpawnPosition;
        }
    }
}