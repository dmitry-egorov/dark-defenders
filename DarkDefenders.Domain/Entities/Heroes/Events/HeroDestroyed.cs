using DarkDefenders.Domain.Infrastructure;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Heroes.Events
{
    internal class HeroDestroyed : Destroyed<Hero>
    {
        public HeroDestroyed(Hero root, IStorage<Hero> storage) : base(root, storage)
        {
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Hero> id)
        {
            reciever.HeroDestroyed();
        }
    }
}