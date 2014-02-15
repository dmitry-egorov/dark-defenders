using DarkDefenders.Domain.Model.Entities.Worlds;
using DarkDefenders.Domain.Resources.Internals;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Resources
{
    public class WorldPropertiesResources : IResources<WorldProperties>
    {
        public WorldProperties this[string resourceId]
        {
            get
            {
                var terrainData = TerrainDataCache.Get(resourceId);

                return new WorldProperties(terrainData.PlayerSpawns, terrainData.HeroSpawns);
            }
        }
    }
}