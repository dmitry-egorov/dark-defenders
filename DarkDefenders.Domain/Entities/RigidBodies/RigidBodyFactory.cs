using System.Collections.Generic;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.RigidBodies.Events;
using DarkDefenders.Domain.Entities.Terrains;
using DarkDefenders.Dtos.Entities.RigidBodies;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Entities.RigidBodies
{
    internal class RigidBodyFactory
    {
        private readonly IStorage<RigidBody> _storage;
        private readonly IContainer<Clock> _clockContainer;
        private readonly IContainer<Terrain> _terrainContainer;

        public RigidBodyFactory(IStorage<RigidBody> storage, IContainer<Clock> clockContainer, IContainer<Terrain> terrainContainer)
        {
            _storage = storage;
            _clockContainer = clockContainer;
            _terrainContainer = terrainContainer;
        }

        public IEnumerable<IEvent> Create(IStorage<RigidBody> additionalStorage, RigidBodyInitialProperties properties)
        {
            var storage = _storage.ComposeWith(additionalStorage);

            return new RigidBodyCreated(storage, _clockContainer, _terrainContainer, properties).EnumerateOnce();
        }
    }
}