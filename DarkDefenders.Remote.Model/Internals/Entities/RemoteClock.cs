using System;
using DarkDefenders.Domain.Model.Events;

namespace DarkDefenders.Remote.Model.Internals.Entities
{
    public class RemoteClock: IClockEvents
    {
        private readonly RemoteEventsPacker _packer;

        public RemoteClock(RemoteEventsPacker packer)
        {
            _packer = packer;
        }

        public void TimeChanged(TimeSpan newTime)
        {
            _packer.Tick(newTime);
        }

        public void Destroyed()
        {
            
        }

        public void Created()
        {
        }
    }
}