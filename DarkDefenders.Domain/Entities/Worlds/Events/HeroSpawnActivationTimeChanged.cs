using System;
using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Dtos.Entities.Worlds;
using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Domain.Entities.Worlds.Events
{
    internal class HeroSpawnActivationTimeChanged : DomainEvent<World, WorldId>
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

        protected override IEventDto CreateDto(WorldId id)
        {
            return new HeroSpawnActivationTimeChangedDto(id, _time);
        }
    }
}