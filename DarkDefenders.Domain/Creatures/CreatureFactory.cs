using System.Collections.Generic;
using DarkDefenders.Domain.Clocks;
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
        public CreatureFactory
        (
            IRepository<CreatureId, Creature> creatureRepository, 
            IRepository<WorldId, World> worldRepository, 
            IRepository<RigidBodyId, RigidBody> rigidBodyRepository, 
            IRepository<ClockId, Clock> clockRepository, 
            RigidBodyFactory rigidBodyFactory, 
            ProjectileFactory projectileFactory
        ): base(creatureRepository)
        {
            _worldRepository = worldRepository;
            _rigidBodyFactory = rigidBodyFactory;
            _projectileFactory = projectileFactory;
            _clockRepository = clockRepository;
            _rigidBodyRepository = rigidBodyRepository;
        }

        public IEnumerable<IDomainEvent> Create(CreatureId creatureId, ClockId clockId, WorldId worldId, Vector spawnPosition, CreatureProperties creatureProperties)
        {
            AssertDoesntExist(creatureId);

            var rigidBodyId = new RigidBodyId();

            var events = CreateCreatureRigidBody(rigidBodyId, clockId, worldId, spawnPosition, creatureProperties.RigidBodyProperties);

            foreach (var e in events) { yield return e; }

            yield return new CreatureCreated(creatureId, clockId, worldId, rigidBodyId, creatureProperties);
        }

        protected override Creature Handle(CreatureCreated creationEvent)
        {
            var clock = _clockRepository.GetById(creationEvent.ClockId);
            var world = _worldRepository.GetById(creationEvent.WorldId);

            var rigidBody = _rigidBodyRepository.GetById(creationEvent.RigidBodyId);
            
            return new Creature(creationEvent.RootId, _projectileFactory, clock, world, rigidBody, creationEvent.Properties);
        }

        private IEnumerable<IDomainEvent> CreateCreatureRigidBody(RigidBodyId rigidBodyId, ClockId clockId, WorldId worldId, Vector position, RigidBodyProperties properties)
        {
            var initialMomentum = Momentum.Zero;
            return _rigidBodyFactory.CreateRigidBody(rigidBodyId, clockId, worldId, initialMomentum, position, properties);
        }

        private readonly IRepository<WorldId, World> _worldRepository;
        private readonly IRepository<RigidBodyId, RigidBody> _rigidBodyRepository;
        private readonly RigidBodyFactory _rigidBodyFactory;
        private readonly ProjectileFactory _projectileFactory;
        private readonly IRepository<ClockId, Clock> _clockRepository;
    }
}