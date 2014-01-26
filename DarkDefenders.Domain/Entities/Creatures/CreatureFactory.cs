using System.Collections.Generic;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Creatures.Events;
using DarkDefenders.Domain.Entities.Projectiles;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Entities.Terrains;
using DarkDefenders.Dtos.Entities.Creatures;
using DarkDefenders.Dtos.Entities.RigidBodies;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using Infrastructure.Util;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Entities.Creatures
{
    [UsedImplicitly]
    internal class CreatureFactory
    {
        private readonly IStorage<Creature> _storage;
        private readonly RigidBodyFactory _rigidBodyFactory;
        private readonly ProjectileFactory _projectileFactory;
        private readonly IContainer<Clock> _clockContainer;
        private readonly IContainer<Terrain> _terrainContainer;

        public CreatureFactory
        (
            IStorage<Creature> storage, 
            RigidBodyFactory rigidBodyFactory, 
            ProjectileFactory projectileFactory, 
            IContainer<Clock> clockContainer, 
            IContainer<Terrain> terrainContainer
        )
        {
            _storage = storage;
            _rigidBodyFactory = rigidBodyFactory;
            _projectileFactory = projectileFactory;
            _clockContainer = clockContainer;
            _terrainContainer = terrainContainer;
        }

        public IEnumerable<IEvent> Create(IStorage<Creature> additionalStorage, Vector spawnPosition, CreatureProperties properties)
        {
            var container = new Container<RigidBody>();

            var rigidBodyInitialProperties = new RigidBodyInitialProperties(Momentum.Zero, spawnPosition, properties.RigidBodyProperties);
            var events = _rigidBodyFactory.Create(container, rigidBodyInitialProperties);

            var storage = _storage.ComposeWith(additionalStorage);
            return events.ConcatItem(new CreatureCreated(storage, _projectileFactory, _clockContainer, _terrainContainer, container, properties));
        }
    }
}