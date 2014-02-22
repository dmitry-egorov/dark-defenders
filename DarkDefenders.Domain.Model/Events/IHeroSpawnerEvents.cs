using System.Collections.ObjectModel;
using DarkDefenders.Domain.Model.Entities;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Events
{
    public interface IHeroSpawnerEvents : IEntityEvents
    {
        void Created(ReadOnlyCollection<HeroSpawnPoint> spawnPoints);
    }
}