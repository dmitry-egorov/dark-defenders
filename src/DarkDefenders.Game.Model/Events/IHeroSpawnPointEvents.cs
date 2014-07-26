using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Game.Model.Events
{
    public interface IHeroSpawnPointEvents : IEntityEvents
    {
        void Created(Vector position);
        void SpawnHeroesChanged(bool enabled);
        void QueuedForSpawnCountChanged(int newQueuedForSpawnCount);
    }
}