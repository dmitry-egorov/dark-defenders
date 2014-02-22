using System;
using System.Threading.Tasks;
using Infrastructure.Util;

namespace Infrastructure.Runtime
{
    public class Loop
    {
        private readonly TimeSpan _elapsedLimit;
        private readonly TimeFiller _timeFiller;
        private volatile bool _stopped;
        
        public Loop(int maxFps, TimeSpan elapsedLimit)
        {
            _elapsedLimit = elapsedLimit;

            var minFrameElapsed = TimeSpan.FromSeconds(1.0 / maxFps);

            _timeFiller = new TimeFiller(minFrameElapsed);
        }

        public Task RunParallel(Action<TimeSpan> action)
        {
            return Task.Factory.StartNew(() => Run(action), TaskCreationOptions.LongRunning);
        }

        public void Run(Action<TimeSpan> action)
        {
            _timeFiller.Start();

            var sw = AutoResetStopwatch.StartNew();

            while (!_stopped)
            {
                action(sw.ElapsedSinceLastCall.LimitTo(_elapsedLimit));

                _timeFiller.FillTimeFrame();
            }
        }

        public void Stop()
        {
            _stopped = true;
        }
    }
}