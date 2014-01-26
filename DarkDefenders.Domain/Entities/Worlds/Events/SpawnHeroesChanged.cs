using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Dtos.Entities.Worlds;
using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Domain.Entities.Worlds.Events
{
    internal class SpawnHeroesChanged : DomainEvent<World, WorldId>
    {
        private readonly bool _enabled;

        public SpawnHeroesChanged(World world, bool enabled) : base(world)
        {
            _enabled = enabled;
        }

        protected override void Apply(World world)
        {
            world.SetSpawnHeroes(_enabled);
        }

        protected override IEventDto CreateDto(WorldId id)
        {
            return new SpawnHeroesChangedDto(id, _enabled);
        }
    }
}