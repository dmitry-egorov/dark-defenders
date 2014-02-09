using DarkDefenders.Domain.Data.Entities.Worlds;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Worlds.Events
{
    internal class SpawnHeroesChanged : Event<World>
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

        protected override object CreateData(IdentityOf<World> id)
        {
            return new SpawnHeroesChangedData(id, _enabled);
        }
    }
}