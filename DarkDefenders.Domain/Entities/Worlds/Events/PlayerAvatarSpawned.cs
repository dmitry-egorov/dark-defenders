using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Infrastructure;

using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Worlds.Events
{
    internal class PlayerAvatarSpawned : EventOf<World>
    {
        private readonly IContainer<Creature> _creatureContainer;

        public PlayerAvatarSpawned(World world, IContainer<Creature> creatureContainer) : base(world)
        {
            _creatureContainer = creatureContainer;
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<World> id)
        {
            reciever.PlayerAvatarSpawned(_creatureContainer.Item.Id);
        }

        protected override void Apply(World world)
        {
        }
    }
}