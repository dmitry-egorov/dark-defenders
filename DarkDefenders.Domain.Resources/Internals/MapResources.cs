using DarkDefenders.Domain.Model.Other;
using DarkDefenders.Domain.Resources.Internals;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Resources
{
    internal class MapResources : IResources<Map<Tile>>
    {
        public Map<Tile> this[string resourceId]
        {
            get
            {
                var terrainData = WorldDataCache.Get(resourceId);

                return terrainData.Map;
            }
        }
    }
}