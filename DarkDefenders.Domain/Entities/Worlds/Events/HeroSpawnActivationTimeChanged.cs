using System;
using DarkDefenders.Domain.Data.Entities.Worlds;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Worlds.Events
{
    internal class HeroSpawnActivationTimeChanged : Event<World>
    {
        private readonly TimeSpan _time;

        public HeroSpawnActivationTimeChanged(World world, TimeSpan time) : base(world)
        {
            _time = time;
        }

        protected override void Apply(World world)
        {
            world.SetHeroSpawnActivationTime(_time);
        }

        protected override object CreateData(IdentityOf<World> id)
        {
            return new HeroSpawnActivationTimeChangedData(id, _time);
        }
    }
}