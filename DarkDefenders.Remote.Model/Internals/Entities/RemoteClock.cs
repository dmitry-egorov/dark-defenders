using System;
using DarkDefenders.Domain.Model.Events;

namespace DarkDefenders.Remote.Model.Internals.Entities
{
    internal class RemoteClock: IClockEvents
    {
        private readonly RemoteEventsPacker _packer;

        public RemoteClock(RemoteEventsPacker packer)
        {
            _packer = packer;
        }

        public void TimeChanged(TimeSpan newTime)
        {
            _packer.Pack();
        }

        public void Destroyed()
        {
            
        }

        public void Created()
        {
        }
    }
}