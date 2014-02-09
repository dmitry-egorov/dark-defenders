using DarkDefenders.Domain.Data.Entities.Worlds;
using DarkDefenders.Domain.Entities.Creatures;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Worlds.Events
{
    internal class PlayerAvatarSpawned : Event<World>
    {
        private readonly IContainer<Creature> _creatureContainer;

        public PlayerAvatarSpawned(World world, IContainer<Creature> creatureContainer) : base(world)
        {
            _creatureContainer = creatureContainer;
        }

        protected override void Apply(World world)
        {
        }

        protected override object CreateData(IdentityOf<World> id)
        {
            var creatureId = _creatureContainer.Item.Id;
            return new PlayerAvatarSpawnedData(id, creatureId);
        }
    }
}