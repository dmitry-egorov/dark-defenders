using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Dtos.Entities.Heroes;
using DarkDefenders.Dtos.Infrastructure;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Heroes.Events
{
    internal class HeroDestroyed : Destroyed<Hero, HeroId>
    {
        public HeroDestroyed(Hero root, IStorage<Hero> storage) : base(root, storage)
        {
        }

        protected override IEventDto CreateDto(HeroId id)
        {
            return new HeroDestroyedDto(id);
        }
    }
}