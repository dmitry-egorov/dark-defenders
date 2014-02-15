using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Model.Entities.HeroSpawnPoints.Events
{
    internal class HeroSpawnPointCreated : Created<HeroSpawnPoint>
    {
        private readonly Vector _position;

        public HeroSpawnPointCreated(HeroSpawnPoint entity, IStorage<HeroSpawnPoint> storage, Vector position) : base(entity, storage)
        {
            _position = position;
        }

        protected override void ApplyTo(HeroSpawnPoint entity)
        {
            entity.Created(_position);
        }

        public override void Accept(IEventsReciever reciever)
        {
        }
    }
}