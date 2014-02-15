using System.Collections.Generic;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Resources.Internals
{
    public static class TerrainDataCache
    {
        private static readonly Dictionary<string, TerrainData> _map = new Dictionary<string, TerrainData>();

        public static TerrainData Get(string resourceId)
        {
            return _map.GetOrCreate(resourceId, () => TerrainLoader.LoadFromFile(resourceId));
        }
    }
}