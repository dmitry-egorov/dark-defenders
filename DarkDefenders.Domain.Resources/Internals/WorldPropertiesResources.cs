using DarkDefenders.Domain.Model.EntityProperties;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Resources.Internals
{
    internal class WorldPropertiesResources : IResources<WorldProperties>
    {
        public WorldProperties this[string resourceId]
        {
            get
            {
                var terrainData = WorldDataCache.Get(resourceId);

                return new WorldProperties(terrainData.PlayerSpawns, terrainData.HeroSpawns);
            }
        }
    }
}