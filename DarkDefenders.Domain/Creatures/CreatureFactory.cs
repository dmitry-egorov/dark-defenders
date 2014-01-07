using System.Collections.Generic;
using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Creatures.Events;
using DarkDefenders.Domain.Projectiles;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Terrains;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Creatures
{
    public class CreatureFactory: RootFactory<CreatureId, Creature, CreatureCreated>
    {
        public CreatureFactory
        (IRepository<ClockId, Clock> clockRepository, IRepository<TerrainId, Terrain> terrainRepository, IRepository<CreatureId, Creature> creatureRepository, IRepository<RigidBodyId, RigidBody> rigidBodyRepository, RigidBodyFactory rigidBodyFactory, ProjectileFactory projectileFactory): base(creatureRepository)
        {
            _rigidBodyFactory = rigidBodyFactory;
            _projectileFactory = projectileFactory;
            _terrainRepository = terrainRepository;
            _clockRepository = clockRepository;
            _rigidBodyRepository = rigidBodyRepository;
        }

        public IEnumerable<IDomainEvent> Create(CreatureId creatureId, RigidBodyId rigidBodyId, ClockId clockId, TerrainId worldId, Vector spawnPosition, CreatureProperties creatureProperties)
        {
            AssertDoesntExist(creatureId);

            var events = CreateCreatureRigidBody(rigidBodyId, clockId, worldId, spawnPosition, creatureProperties.RigidBodyProperties);

            foreach (var e in events) { yield return e; }

            yield return new CreatureCreated(creatureId, clockId, worldId, rigidBodyId, creatureProperties);
        }

        protected override Creature Handle(CreatureCreated creationEvent)
        {
            var clock = _clockRepository.GetById(creationEvent.ClockId);
            var rigidBody = _rigidBodyRepository.GetById(creationEvent.RigidBodyId);
            var terrainId = _terrainRepository.GetById(creationEvent.TerrainId);

            return new Creature(creationEvent.RootId, _projectileFactory, clock, terrainId, rigidBody, creationEvent.Properties);
        }

        private IEnumerable<IDomainEvent> CreateCreatureRigidBody(RigidBodyId rigidBodyId, ClockId clockId, TerrainId terrainId, Vector position, RigidBodyProperties properties)
        {
            var initialMomentum = Momentum.Zero;
            return _rigidBodyFactory.CreateRigidBody(rigidBodyId, clockId, terrainId, initialMomentum, position, properties);
        }

        private readonly IRepository<RigidBodyId, RigidBody> _rigidBodyRepository;
        private readonly RigidBodyFactory _rigidBodyFactory;
        private readonly ProjectileFactory _projectileFactory;
        private readonly IRepository<ClockId, Clock> _clockRepository;
        private readonly IRepository<TerrainId, Terrain> _terrainRepository;
    }
}