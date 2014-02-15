using System;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.HeroSpawnPoints.Events
{
    internal class ActivationTimeChanged : EventOf<HeroSpawnPoint>
    {
        private readonly TimeSpan _time;

        public ActivationTimeChanged(HeroSpawnPoint heroSpawnPoint, TimeSpan time)
            : base(heroSpawnPoint)
        {
            _time = time;
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<HeroSpawnPoint> id)
        {
        }

        protected override void Apply(HeroSpawnPoint world)
        {
            world.ActivationTimeChanged(_time);
        }
    }
}