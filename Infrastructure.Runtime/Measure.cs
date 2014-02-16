using System;
using System.Diagnostics;

namespace Infrastructure.Runtime
{
    public static class Measure
    {
        public static TimeSpan Time(Action action)
        {
            var sw = Stopwatch.StartNew();

            action();

            return sw.Elapsed;
        }
    }
}