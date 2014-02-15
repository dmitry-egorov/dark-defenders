using System.Collections.Generic;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Entities.RigidBodies.Events;
using DarkDefenders.Domain.Entities.Terrains;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
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

        public ICreation<RigidBody> Create(Vector initialPosition, Momentum initialMomentum, RigidBodyProperties properties)
        {
            return GetCreation(s => YieldEvents(s, initialPosition, initialMomentum, properties));
        }

        private IEnumerable<IEvent> YieldEvents(IStorage<RigidBody> storage, Vector initialPosition, Momentum initialMomentum, RigidBodyProperties properties)
        {
            yield return new RigidBodyCreated(storage, _clockContainer, _terrainContainer, initialPosition, initialMomentum, properties);
        }
    }
}