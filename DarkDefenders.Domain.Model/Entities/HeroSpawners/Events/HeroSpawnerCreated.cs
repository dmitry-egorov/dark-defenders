using System.Collections.ObjectModel;
using DarkDefenders.Domain.Model.Entities.HeroSpawnPoints;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.HeroSpawners.Events
{
    internal class HeroSpawnerCreated : Created<HeroSpawner>
    {
        private readonly ReadOnlyCollection<HeroSpawnPoint> _spawnPoints;

        public HeroSpawnerCreated(HeroSpawner entity, IStorage<HeroSpawner> storage, ReadOnlyCollection<HeroSpawnPoint> spawnPoints) : base(entity, storage)
        {
            _spawnPoints = spawnPoints;
        }

        protected override void ApplyTo(HeroSpawner entity)
        {
            entity.Created(_spawnPoints);
        }

        public override void Accept(IEventsReciever reciever)
        {
        }
    }
}