using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Heroes.Events
{
    internal class HeroDestroyed : Destroyed<Hero>
    {
        public HeroDestroyed(Hero entity, IStorage<Hero> storage) : base(entity, storage)
        {
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Hero> id)
        {
            reciever.HeroDestroyed();
        }
    }
}