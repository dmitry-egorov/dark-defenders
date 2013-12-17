using System;
using System.Diagnostics;

namespace Infrastructure.Concurrent
{
    public static class Spin
    {
        public static void Wait(TimeSpan timeToSpin)
        {
            if (timeToSpin.TotalSeconds < 0)
            {
                throw new ArgumentException("Time to spin is negative", "timeToSpin");
            }

            var sw = Stopwatch.StartNew();
            while (sw.Elapsed < timeToSpin)
            {
                
            }
        }
    }
}