using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Worlds.Events
{
    internal class SpawnHeroesChanged : EventOf<World>
    {
        private readonly bool _enabled;

        public SpawnHeroesChanged(World world, bool enabled) : base(world)
        {
            _enabled = enabled;
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<World> id)
        {
        }

        protected override void Apply(World world)
        {
            world.SpawnHeroesChanged(_enabled);
        }
    }
}