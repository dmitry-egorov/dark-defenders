using System;
using System.Diagnostics;

namespace DarkDefenders.Console
{
    public class FixedTimeFrameExecutor
    {
        private readonly Stopwatch _sw = new Stopwatch();
        private readonly TimeSpan _minFrameElapsed;
        private bool _started;

        public static FixedTimeFrameExecutor StartNew(TimeSpan minFrameElapsed)
        {
            var executor = new FixedTimeFrameExecutor(minFrameElapsed);
            executor.Start();
            return executor;
        }

        public FixedTimeFrameExecutor(TimeSpan minFrameElapsed)
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