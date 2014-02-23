using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Util;

namespace Infrastructure.Runtime
{
    public class Loop
    {
        private readonly TimeSpan _elapsedLimit;
        private readonly TimeFiller _timeFiller;
        
        public Loop(int maxFps, TimeSpan elapsedLimit)
        {
            _elapsedLimit = elapsedLimit;

            var minFrameElapsed = TimeSpan.FromSeconds(1.0 / maxFps);

            _timeFiller = new TimeFiller(minFrameElapsed);
        }

        public Task RunParallel(Action<TimeSpan> action, CancellationToken token)
        {
            return Task.Factory.StartNew(() => Run(action, token), TaskCreationOptions.LongRunning);
        }

        public void Run(Action<TimeSpan> action, CancellationToken token)
        {
            _timeFiller.Start();

            var sw = AutoResetStopwatch.StartNew();

            while (!token.IsCancellationRequested)
            {
                action(sw.ElapsedSinceLastCall.LimitTo(_elapsedLimit));

                _timeFiller.FillTimeFrame();
            }
        }
    }
}