using DarkDefenders.Domain.Model.Entities.Creatures;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Worlds.Events
{
    internal class PlayerAvatarSpawned : EventOf<World>
    {
        private readonly Creature _creature;

        public PlayerAvatarSpawned(World world, Creature creature) : base(world)
        {
            _creature = creature;
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<World> id)
        {
            reciever.PlayerCreated(_creature.Id);
        }

        protected override void Apply(World world)
        {
        }
    }
}