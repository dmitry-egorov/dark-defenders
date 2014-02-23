using System.Collections.Generic;

namespace Infrastructure.Util
{
    public static class As
    {
        public static IEnumerable<T> Enumerable<T>(params T[] items)
        {
            return items;
        }
    }
}