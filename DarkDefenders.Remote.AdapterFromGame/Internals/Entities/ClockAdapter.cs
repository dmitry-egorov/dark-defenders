using System;
using DarkDefenders.Game.Model.Events;

namespace DarkDefenders.Remote.AdapterFromGame.Internals.Entities
{
    internal class ClockAdapter : IClockEvents
    {
        private readonly RemoteEventsPacker _packer;

        public ClockAdapter(RemoteEventsPacker packer)
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