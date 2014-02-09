using System;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Worlds.Events
{
    internal class HeroSpawnActivationTimeChanged : EventOf<World>
    {
        private readonly TimeSpan _time;

        public HeroSpawnActivationTimeChanged(World world, TimeSpan time) : base(world)
        {
            _time = time;
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<World> id)
        {
        }

        protected override void Apply(World world)
        {
            world.HeroSpawnActivationTimeChanged(_time);
        }
    }
}