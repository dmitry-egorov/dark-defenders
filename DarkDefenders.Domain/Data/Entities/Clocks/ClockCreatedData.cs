using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.Clocks;
using Infrastructure.Data;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Clocks
{
    [ProtoContract]
    public class ClockCreatedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<Clock> ClockId { get; private set; }

        private ClockCreatedData()//Protobuf
        {
        }

        public ClockCreatedData(IdentityOf<Clock> clockId)
        {
            ClockId = clockId;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}