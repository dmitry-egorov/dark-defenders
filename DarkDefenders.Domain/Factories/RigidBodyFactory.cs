using System.Collections.Generic;
using DarkDefenders.Domain.Data.Entities.RigidBodies;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Entities.RigidBodies.Events;
using DarkDefenders.Domain.Entities.Terrains;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Factories
{
    [UsedImplicitly]
    internal class RigidBodyFactory: Factory<RigidBody>
    {
        private readonly IContainer<Clock> _clockContainer;
        private readonly IContainer<Terrain> _terrainContainer;

        public RigidBodyFactory(IStorage<RigidBody> storage, IContainer<Clock> clockContainer, IContainer<Terrain> terrainContainer)
            : base(storage)
        {
            _clockContainer = clockContainer;
            _terrainContainer = terrainContainer;
        }

        public ICreation<RigidBody> Create(RigidBodyInitialProperties properties)
        {
            return GetCreation(s => YieldEvents(s, properties));
        }

        private IEnumerable<IEvent> YieldEvents(IStorage<RigidBody> storage, RigidBodyInitialProperties properties)
        {
            yield return new RigidBodyCreated(storage, _clockContainer, _terrainContainer, properties);
        }
    }
}