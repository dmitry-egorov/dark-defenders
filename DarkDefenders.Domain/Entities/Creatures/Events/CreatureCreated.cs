using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Projectiles;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Entities.Terrains;
using DarkDefenders.Dtos.Entities.Creatures;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Creatures.Events
{
    internal class CreatureCreated : Created<Creature, CreatureId>
    {
        private readonly IStorage<Creature> _storage;
        private readonly ProjectileFactory _projectileFactory;
        private readonly IContainer<Clock> _clockContainer;
        private readonly IContainer<Terrain> _terrainContainer;
        private readonly CreatureProperties _properties;
        private readonly IContainer<RigidBody> _rigidBodyContainer;

        public CreatureCreated(IStorage<Creature> storage, ProjectileFactory projectileFactory, IContainer<Clock> clockContainer, IContainer<Terrain> terrainContainer, IContainer<RigidBody> rigidBodyContainer, CreatureProperties properties) 
            : base(storage)
        {
            _storage = storage;
            _projectileFactory = projectileFactory;
            _clockContainer = clockContainer;
            _terrainContainer = terrainContainer;
            _properties = properties;
            _rigidBodyContainer = rigidBodyContainer;
        }


        protected override object CreateDto(CreatureId creatureId)
        {
            var rigidBodyId = _rigidBodyContainer.Item.GetGlobalId();

            return new CreatureCreatedDto(creatureId, rigidBodyId, _properties);
        }

        protected override Creature Create()
        {
            var rigidBody = _rigidBodyContainer.Item;

            return new Creature(_storage, _projectileFactory, _clockContainer, _terrainContainer, rigidBody, _properties);
        }
    }
}