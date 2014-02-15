using System;

namespace Infrastructure.Runtime
{
    public class Loop
    {
        private readonly TimeFiller _timeFiller;
        private volatile bool _stopped;

        public Loop(int maxFps)
        {
            var minFrameElapsed = TimeSpan.FromSeconds(1.0 / maxFps);

            _timeFiller = new TimeFiller(minFrameElapsed);
        }

        public void Run(Action action)
        {
            _timeFiller.Start();

            while (!_stopped)
            {
                action();

                _timeFiller.FillTimeFrame();
            }
        }

        public void Stop()
        {
            _stopped = true;
        }
    }
}