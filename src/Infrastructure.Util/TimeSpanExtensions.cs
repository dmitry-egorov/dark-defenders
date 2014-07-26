using System;

namespace Infrastructure.Util
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan LimitTo(this TimeSpan timeSpan, TimeSpan limit)
        {
            return timeSpan > limit ? limit : timeSpan;
        }
    }
}