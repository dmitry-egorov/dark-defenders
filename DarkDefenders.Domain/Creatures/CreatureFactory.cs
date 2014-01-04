using System.Collections.Generic;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Creatures.Events;
using DarkDefenders.Domain.Projectiles;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Creatures
{
    public class CreatureFactory: RootFactory<CreatureId, Creature, CreatureCreated>
    {
        private readonly IRepository<WorldId, World> _worldRepository;
        private readonly IRepository<RigidBodyId, RigidBody> _rigidBodyRepository;
        private readonly RigidBodyFactory _rigidBodyFactory;
        private readonly ProjectileFactory _projectileFactory;

        public CreatureFactory(IRepository<CreatureId, Creature> creatureRepository, IRepository<WorldId, World> worldRepository, IRepository<RigidBodyId, RigidBody> rigidBodyRepository, RigidBodyFactory rigidBodyFactory, ProjectileFactory projectileFactory): base(creatureRepository)
        {
            _worldRepository = worldRepository;
            _rigidBodyFactory = rigidBodyFactory;
            _projectileFactory = projectileFactory;
            _rigidBodyRepository = rigidBodyRepository;
        }

        public IEnumerable<IDomainEvent> Create(CreatureId creatureId, WorldId worldId)
        {
            AssertDoesntExist(creatureId);

            var world = _worldRepository.GetById(worldId);

            var spawnPosition = world.GetSpawnPosition();

            var rigidBodyId = new RigidBodyId();

            var events = CreateCreatureRigidBody(rigidBodyId, worldId, spawnPosition);

            foreach (var e in events) { yield return e; }

            yield return new CreatureCreated(creatureId, worldId, rigidBodyId);
        }

        protected override Creature Handle(CreatureCreated creationEvent)
        {
            var world = _worldRepository.GetById(creationEvent.WorldId);

            var rigidBody = _rigidBodyRepository.GetById(creationEvent.RigidBodyId);
            
            return new Creature(creationEvent.RootId, _projectileFactory, world, rigidBody);
        }

        private IEnumerable<IDomainEvent> CreateCreatureRigidBody(RigidBodyId rigidBodyId, WorldId worldId, Vector spawnPosition)
        {
            var radius = Creature.BoundingBoxRadius;
            var boundingBox = new Box(spawnPosition, radius, radius);
            var initialMomentum = Vector.Zero;
            var mass = Creature.Mass;
            var topHorizontalMomentum = Creature.TopHorizontalMomentum;

            return _rigidBodyFactory.CreateRigidBody(rigidBodyId, worldId, initialMomentum, mass, topHorizontalMomentum, boundingBox);
        }
    }
}