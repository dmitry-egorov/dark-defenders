using DarkDefenders.Domain.Model.Entities.Creatures;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Heroes.Events
{
    internal class HeroCreated: Created<Hero>
    {
        private readonly Creature _creature;

        public HeroCreated(IStorage<Hero> storage, Hero hero, Creature creature) : base(hero, storage)
        {
            _creature = creature;
        }

        public override void Accept(IEventsReciever reciever)
        {
            reciever.HeroCreated(_creature.Id);
        }

        protected override void ApplyTo(Hero hero)
        {
            hero.Created(_creature);
        }
    }
}