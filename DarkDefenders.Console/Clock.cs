using System;
using System.Diagnostics;

namespace DarkDefenders.Console
{
    public class Clock
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private TimeSpan _last = TimeSpan.Zero;

        public static Clock StartNew()
        {
            var clock = new Clock();
            clock.Start();
            return clock;
        }

        public void Start()
        {
            _stopwatch.Start();
        }

        public TimeSpan ElapsedSinceLastCall
        {
            get
            {
                var current = _stopwatch.Elapsed;
                var elapsed = current - _last;
                _last = current;

                return elapsed;
            }
        }
    }
}