using DarkDefenders.Domain.Players;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES.Implementations;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain
{
    public static class Configurator
    {
        public static void ConfigureDomain(this CommandProcessor processor)
        {
            var worldRepository = new Repository<WorldId, World>(id => new World(id));
            var rigidBodyRepository = new Repository<RigidBodyId, RigidBody>(id => new RigidBody(id, worldRepository));
            var playerRepository  = new Repository<PlayerId, Player> (id => new Player(id, worldRepository, rigidBodyRepository));

            processor.AddRepository<WorldId, World, IWorldEvent>(worldRepository);
            processor.AddRepository<RigidBodyId, RigidBody, IRigidBodyEvent>(rigidBodyRepository);
            processor.AddRepository<PlayerId, Player, IPlayerEvent>(playerRepository);
        }
    }
}