using System;

namespace Infrastructure.Runtime
{
    public class LoopRunner
    {
        private readonly TimeFiller _timeFiller;
        private volatile bool _stopped;
        private readonly Action _action;

        public LoopRunner(int maxFps, Action action)
        {
            var minFrameElapsed = TimeSpan.FromSeconds(1.0 / maxFps);

            _action = action;

            _timeFiller = new TimeFiller(minFrameElapsed);
        }

        public void Run()
        {
            _timeFiller.Start();

            while (!_stopped)
            {
                _action();

                _timeFiller.FillTimeFrame();
            }
        }

        public void Stop()
        {
            _stopped = true;
        }
    }
}