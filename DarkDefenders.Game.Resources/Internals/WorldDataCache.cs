using System.Collections.Generic;
using Infrastructure.Util;

namespace DarkDefenders.Game.Resources.Internals
{
    public static class WorldDataCache
    {
        private static readonly Dictionary<string, WorldData> _map = new Dictionary<string, WorldData>();

        public static WorldData Get(string resourceId)
        {
            return _map.GetOrCreate(resourceId, () => WorldLoader.LoadFromFile(resourceId));
        }
    }
}