using DarkDefenders.Domain.Players;
using DarkDefenders.Domain.Terrains;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain
{
    public static class Configurator
    {
        public static void ConfigureDomain(this CommandProcessor processor, IEventStore eventStore)
        {
            var terrainRepository = new Repository<Terrain, ITerrainEvent, ITerrainEventsReciever, TerrainId>(eventStore, id => new Terrain(id));
            var playerRepository  = new Repository<Player, IPlayerEvent, IPlayerEventsReciever, PlayerId> (eventStore, id => new Player(id, terrainRepository));

            processor.AddRepository(terrainRepository);
            processor.AddRepository(playerRepository);
        }
    }
}