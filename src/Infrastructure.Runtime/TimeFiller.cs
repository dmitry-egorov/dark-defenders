using System;
using System.Diagnostics;

namespace Infrastructure.Runtime
{
    public class TimeFiller
    {
        private readonly Stopwatch _sw = new Stopwatch();
        private readonly TimeSpan _minFrameElapsed;
        private bool _started;

        public static TimeFiller StartNew(TimeSpan minFrameElapsed)
        {
            var executor = new TimeFiller(minFrameElapsed);
            executor.Start();
            return executor;
        }

        public TimeFiller(TimeSpan minFrameElapsed)
        {
            _minFrameElapsed = minFrameElapsed;
        }

        public void Start()
        {
            _sw.Start();
            _started = true;
        }

        public void FillTimeFrame()
        {
            if (!_started)
            {
                throw new InvalidOperationException("Executor is not started.");
            }

            var elapsed = _sw.Elapsed;
            _sw.Restart();

            var timeLeft = _minFrameElapsed - elapsed;
            if (timeLeft.TotalSeconds > 0)
            {
                while (_sw.Elapsed < timeLeft)
                {
                }
            }

            _sw.Restart();
        }
    }
}