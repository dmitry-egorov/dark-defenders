using System;

namespace Infrastructure.Math.Physics
{
    public static class SecondsExtensions
    {
        public static Seconds ToSeconds(this TimeSpan timeSpan)
        {
            return new Seconds(timeSpan.TotalSeconds);
        }

        public static Seconds ToSeconds(this double seconds)
        {
            return new Seconds(seconds);
        }
    }
}