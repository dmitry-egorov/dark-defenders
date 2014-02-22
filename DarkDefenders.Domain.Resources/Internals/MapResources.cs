using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Resources.Internals
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