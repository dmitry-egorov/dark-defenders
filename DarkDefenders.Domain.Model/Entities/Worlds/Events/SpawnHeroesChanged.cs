using DarkDefenders.Domain.Model.Entities.HeroSpawnPoints;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Worlds.Events
{
    internal class SpawnHeroesChanged : EventOf<HeroSpawnPoint>
    {
        private readonly bool _enabled;

        public SpawnHeroesChanged(HeroSpawnPoint heroSpawnPoint, bool enabled) : base(heroSpawnPoint)
        {
            _enabled = enabled;
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<HeroSpawnPoint> id)
        {
        }

        protected override void Apply(HeroSpawnPoint heroSpawnPoint)
        {
            heroSpawnPoint.SpawnHeroesChanged(_enabled);
        }
    }
}