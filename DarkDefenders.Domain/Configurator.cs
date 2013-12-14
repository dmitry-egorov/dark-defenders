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
            var terrainRepository = new Repository<Terrain, TerrainSnapshot, ITerrainEvent, ITerrainEventsReciever, TerrainId>(eventStore, () => new Terrain());
            var playerRepository  = new Repository<Player,  PlayerSnapshot,  IPlayerEvent,  IPlayerEventsReciever,  PlayerId> (eventStore, () => new Player(terrainRepository));

            processor.AddRepository(terrainRepository);
            processor.AddRepository(playerRepository);
        }
    }
}