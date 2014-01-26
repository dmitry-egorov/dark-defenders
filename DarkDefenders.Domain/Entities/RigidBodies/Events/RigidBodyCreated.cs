using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Terrains;
using DarkDefenders.Dtos.Entities.RigidBodies;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class RigidBodyCreated : Created<RigidBody, RigidBodyId>
    {
        private readonly IStorage<RigidBody> _storage;
        private readonly IContainer<Clock> _clockContainer;
        private readonly IContainer<Terrain> _terrainContainer;
        private readonly RigidBodyInitialProperties _rigidBodyInitialProperties;

        public RigidBodyCreated(IStorage<RigidBody> storage, IContainer<Clock> clockContainer, IContainer<Terrain> terrainContainer, RigidBodyInitialProperties rigidBodyInitialProperties)
            : base(storage)
        {
            _storage = storage;
            _clockContainer = clockContainer;
            _terrainContainer = terrainContainer;
            _rigidBodyInitialProperties = rigidBodyInitialProperties;
        }

        protected override object CreateDto(RigidBodyId rigidBodyId)
        {
            return new RigidBodyCreatedDto(rigidBodyId, _rigidBodyInitialProperties);
        }

        protected override RigidBody Create()
        {
            return new RigidBody(_storage, _clockContainer, _terrainContainer, _rigidBodyInitialProperties);
        }
    }
}