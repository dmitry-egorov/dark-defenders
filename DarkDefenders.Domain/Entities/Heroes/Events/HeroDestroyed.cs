using DarkDefenders.Domain.Data.Entities.Heroes;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Heroes.Events
{
    internal class HeroDestroyed : Destroyed<Hero>
    {
        public HeroDestroyed(Hero root, IStorage<Hero> storage) : base(root, storage)
        {
        }

        protected override object CreateData(IdentityOf<Hero> id)
        {
            return new HeroDestroyedData(id);
        }
    }
}