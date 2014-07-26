using System;

namespace Infrastructure.Util
{
    public static class IntExtensions
    {
        public static int LimitTop(this int value, int limit)
        {
            return Math.Min(value, limit);
        }
    }
}