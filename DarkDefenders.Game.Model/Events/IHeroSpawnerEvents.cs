using System.Collections.ObjectModel;
using DarkDefenders.Game.Model.Entities;
using Infrastructure.DDDES;

namespace DarkDefenders.Game.Model.Events
{
    public interface IHeroSpawnerEvents : IEntityEvents
    {
        void Created(ReadOnlyCollection<HeroSpawnPoint> spawnPoints);
    }
}