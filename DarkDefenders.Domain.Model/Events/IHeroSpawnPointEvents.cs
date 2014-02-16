using System;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Model.Events
{
    public interface IHeroSpawnPointEvents: IEntityEvents
    {
        void Created(Vector position);
        void ActivationTimeChanged(TimeSpan time);
        void SpawnHeroesChanged(bool enabled);
        void QueuedForSpawnCountChanged(int newQueuedForSpawnCount);
    }
}