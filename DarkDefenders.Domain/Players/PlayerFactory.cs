using System.Collections.Generic;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.Projectiles;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Players
{
    public class PlayerFactory: RootFactory<PlayerId, Player, PlayerCreated>
    {
        private readonly IRepository<WorldId, World> _worldRepository;
        private readonly IRepository<RigidBodyId, RigidBody> _rigidBodyRepository;
        private readonly RigidBodyFactory _rigidBodyFactory;
        private readonly ProjectileFactory _projectileFactory;

        public PlayerFactory(IRepository<PlayerId, Player> playerRepository, IRepository<WorldId, World> worldRepository, IRepository<RigidBodyId, RigidBody> rigidBodyRepository, RigidBodyFactory rigidBodyFactory, ProjectileFactory projectileFactory): base(playerRepository)
        {
            _worldRepository = worldRepository;
            _rigidBodyFactory = rigidBodyFactory;
            _projectileFactory = projectileFactory;
            _rigidBodyRepository = rigidBodyRepository;
        }

        public IEnumerable<IEvent> Create(PlayerId playerId, WorldId worldId)
        {
            AssertDoesntExist(playerId);

            var world = _worldRepository.GetById(worldId);

            var spawnPosition = world.GetSpawnPosition();

            var rigidBodyId = new RigidBodyId();

            var events = CreatePlayerRigidBody(rigidBodyId, worldId, spawnPosition);

            foreach (var e in events) { yield return e; }

            yield return new PlayerCreated(playerId, worldId, rigidBodyId);
        }

        protected override Player Handle(PlayerCreated creationEvent)
        {
            var world = _worldRepository.GetById(creationEvent.WorldId);

            var rigidBody = _rigidBodyRepository.GetById(creationEvent.RigidBodyId);
            
            return new Player(creationEvent.RootId, _projectileFactory, world, rigidBody);
        }

        private IEnumerable<IEvent> CreatePlayerRigidBody(RigidBodyId rigidBodyId, WorldId worldId, Vector spawnPosition)
        {
            return _rigidBodyFactory.CreateRigidBody(rigidBodyId, worldId, spawnPosition, Player.BoundingCircleRadius, Vector.Zero, Player.Mass);
        }
    }
}