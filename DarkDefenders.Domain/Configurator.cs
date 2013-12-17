using DarkDefenders.Domain.Players;
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
            var playerRepository  = new Repository<PlayerId, Player> (id => new Player(id, worldRepository));

            processor.AddRepository<WorldId, World, IWorldEvent>(worldRepository);
            processor.AddRepository<PlayerId, Player, IPlayerEvent>(playerRepository);
        }
    }
}