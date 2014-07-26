using DarkDefenders.Game.Model.EntityProperties;
using Infrastructure.DDDES;

namespace DarkDefenders.Game.Resources.Internals
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