using DarkDefenders.Domain.Players;
using DarkDefenders.Domain.Terrains;
using Infrastructure.DDDES.Implementations;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain
{
    public static class Configurator
    {
        public static void ConfigureDomain(this CommandProcessor processor)
        {
            var terrainRepository = new Repository<TerrainId, Terrain, ITerrainEvent, ITerrainEventsReciever>(id => new Terrain(id));
            var playerRepository  = new Repository<PlayerId, Player, IPlayerEvent, IPlayerEventsReciever> (id => new Player(id, terrainRepository));

            processor.AddRepository(terrainRepository);
            processor.AddRepository(playerRepository);
        }
    }
}