using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Dtos.Entities.Worlds;
using DarkDefenders.Dtos.Infrastructure;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Worlds.Events
{
    internal class PlayerAvatarSpawned : DomainEvent<World, WorldId>
    {
        private readonly IContainer<Creature> _creatureContainer;

        public PlayerAvatarSpawned(World world, IContainer<Creature> creatureContainer) : base(world)
        {
            _creatureContainer = creatureContainer;
        }

        protected override void Apply(World world)
        {
        }

        protected override IEventDto CreateDto(WorldId id)
        {
            var creatureId = _creatureContainer.Item.GetGlobalId();
            return new PlayerAvatarSpawnedDto(id, creatureId);
        }
    }
}