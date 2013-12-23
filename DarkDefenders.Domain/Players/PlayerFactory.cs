using System.Collections.Generic;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Players
{
    public class PlayerFactory: Factory<PlayerId, Player>, IFactory<Player, PlayerCreated>
    {
        private readonly IRepository<WorldId, World> _worldRepository;
        private readonly IRepository<RigidBodyId, RigidBody> _rigidBodyRepository;
        private readonly RigidBodyFactory _rigidBodyFactory;

        public PlayerFactory(IRepository<PlayerId, Player> playerRepository, IRepository<WorldId, World> worldRepository, IRepository<RigidBodyId, RigidBody> rigidBodyRepository, RigidBodyFactory rigidBodyFactory): base(playerRepository)
        {
            _worldRepository = worldRepository;
            _rigidBodyFactory = rigidBodyFactory;
            _rigidBodyRepository = rigidBodyRepository;
        }

        Player IFactory<Player, PlayerCreated>.Handle(PlayerCreated creationEvent)
        {
            var world = _worldRepository.GetById(creationEvent.WorldId);

            var rigidBody = _rigidBodyRepository.GetById(creationEvent.RigidBodyId);
            
            return new Player(creationEvent.RootId, _rigidBodyFactory, _rigidBodyRepository, world, rigidBody);
        }

        public IEnumerable<IEvent> Create(PlayerId playerId, WorldId worldId)
        {
            AssertDoesntExist(playerId);
            foreach (var e in CreatePlayer(playerId, worldId)) { yield return e; }
        }

        private IEnumerable<IEvent> CreatePlayer(PlayerId playerId, WorldId worldId)
        {
            var world = _worldRepository.GetById(worldId);

            RigidBodyId rigidBodyId;
            foreach (var e in CreateOwnRigidBody(world, out rigidBodyId)) { yield return e; }

            yield return new PlayerCreated(playerId, worldId, rigidBodyId);
        }

        private IEnumerable<IEvent> CreateOwnRigidBody(World world, out RigidBodyId rigidBodyId)
        {
            var spawnPosition = world.GetSpawnPosition();
            
            rigidBodyId = new RigidBodyId();
            return _rigidBodyFactory.CreateRigidBody(rigidBodyId, world.Id, spawnPosition, Player.BoundingCircleRadius, Vector.Zero, Player.Mass);
        }
    }
}