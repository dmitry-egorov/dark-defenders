using System;

namespace Infrastructure.Util
{
    public static class SingleExtensions
    {
        public static float LimitTop(this float value, float limit)
        {
            return Math.Min(value, limit);
        }
    }
}