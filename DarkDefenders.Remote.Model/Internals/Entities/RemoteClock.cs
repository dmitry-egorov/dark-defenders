using System;
using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Remote.Model.Interface;

namespace DarkDefenders.Remote.Model.Internals.Entities
{
    public class RemoteClock: IClockEvents
    {
        private readonly IRemoteEvents _reciever;

        public RemoteClock(IRemoteEvents reciever)
        {
            _reciever = reciever;
        }

        public void TimeChanged(TimeSpan newTime)
        {
            _reciever.Tick(newTime);
        }

        public void Destroyed()
        {
            
        }

        public void Created()
        {
        }
    }
}