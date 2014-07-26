using System;
using System.Diagnostics;

namespace Infrastructure.Runtime
{
    public class AutoResetStopwatch
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private TimeSpan _last = TimeSpan.Zero;

        public static AutoResetStopwatch StartNew()
        {
            var clock = new AutoResetStopwatch();
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