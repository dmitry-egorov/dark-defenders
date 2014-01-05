using System.Collections.Generic;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Creatures.Events;
using DarkDefenders.Domain.Projectiles;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Math.Physics;

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

        public IEnumerable<IDomainEvent> Create(CreatureId creatureId, WorldId worldId, Vector spawnPosition, RigidBodyProperties properties)
        {
            AssertDoesntExist(creatureId);

            var rigidBodyId = new RigidBodyId();

            var events = CreateCreatureRigidBody(rigidBodyId, worldId, spawnPosition, properties);

            foreach (var e in events) { yield return e; }

            yield return new CreatureCreated(creatureId, worldId, rigidBodyId);
        }

        protected override Creature Handle(CreatureCreated creationEvent)
        {
            var world = _worldRepository.GetById(creationEvent.WorldId);

            var rigidBody = _rigidBodyRepository.GetById(creationEvent.RigidBodyId);
            
            return new Creature(creationEvent.RootId, _projectileFactory, world, rigidBody);
        }

        private IEnumerable<IDomainEvent> CreateCreatureRigidBody(RigidBodyId rigidBodyId, WorldId worldId, Vector position, RigidBodyProperties properties)
        {
            var initialMomentum = Momentum.Zero;
            return _rigidBodyFactory.CreateRigidBody(rigidBodyId, worldId, initialMomentum, position, properties);
        }
    }
}