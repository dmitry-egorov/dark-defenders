using System;
using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.Clocks;
using Infrastructure.Data;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Clocks
{
    [ProtoContract]
    public class TimeChangedData : EventDataBase
    {
        [ProtoMember(2)]
        public TimeSpan NewTime { get; private set; }

        [ProtoMember(1)]
        public IdentityOf<Clock> ClockId { get; private set; }

        private TimeChangedData()//Protobuf
        {
        }

        public TimeChangedData(IdentityOf<Clock> clockId, TimeSpan newTime)
        {
            ClockId = clockId;
            NewTime = newTime;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}