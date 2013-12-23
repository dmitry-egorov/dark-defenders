using DarkDefenders.Domain.Players;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES.Implementations;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain
{
    public static class Configurator
    {
        public static void ConfigureDomain(this CommandProcessor processor)
        {
            var worldRepository = new Repository<WorldId, World>();
            var rigidBodyRepository = new Repository<RigidBodyId, RigidBody>();
            var playerRepository = new Repository<PlayerId, Player>();

            var rigidBodyFactory = new RigidBodyFactory(rigidBodyRepository, worldRepository);
            var worldFactory = new WorldFactory(worldRepository);
            var playerFactory = new PlayerFactory(playerRepository, worldRepository, rigidBodyRepository, rigidBodyFactory);

            processor.AddRepository<WorldId, World, IWorldEvent, WorldFactory, WorldCreated, WorldRemoved>(worldRepository, worldFactory);
            processor.AddRepository<RigidBodyId, RigidBody, IRigidBodyEvent, RigidBodyFactory, RigidBodyCreated, RigidBodyRemoved>(rigidBodyRepository, rigidBodyFactory);
            processor.AddRepository<PlayerId, Player, IPlayerEvent, PlayerFactory, PlayerCreated, PlayerRemoved>(playerRepository, playerFactory);
        }
    }
}