using DarkDefenders.Domain.Data.Entities.RigidBodies;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Terrains;
using DarkDefenders.Domain.Infrastructure;

using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.RigidBodies.Events
{
    internal class RigidBodyCreated : Created<RigidBody>
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

        protected override RigidBody Create()
        {
            return new RigidBody(_storage, _clockContainer, _terrainContainer, _rigidBodyInitialProperties);
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<RigidBody> id)
        {
            reciever.RigidBodyCreated(id, _rigidBodyInitialProperties.Position);
        }
    }
}