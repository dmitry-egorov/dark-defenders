using System.Collections.Generic;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.Creatures.Events;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Entities.Terrains;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Factories
{
    [UsedImplicitly]
    internal class CreatureFactory: Factory<Creature>
    {
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
        ) : base(storage)
        {
            _rigidBodyFactory = rigidBodyFactory;
            _projectileFactory = projectileFactory;
            _clockContainer = clockContainer;
            _terrainContainer = terrainContainer;
        }

        public ICreation<Creature> Create(Vector spawnPosition, CreatureProperties properties)
        {
            return GetCreation(s => YielEvents(s, spawnPosition, properties));
        }

        private IEnumerable<IEvent> YielEvents(IStorage<Creature> storage, Vector spawnPosition, CreatureProperties properties)
        {
            var rigidBodyInitialProperties = new RigidBodyInitialProperties(Momentum.Zero, spawnPosition, properties.RigidBodyProperties);

            var creation = _rigidBodyFactory.Create(rigidBodyInitialProperties);

            foreach (var e in creation) { yield return e; }

            yield return new CreatureCreated(storage, _projectileFactory, _clockContainer, _terrainContainer, creation, properties);
        }
    }
}