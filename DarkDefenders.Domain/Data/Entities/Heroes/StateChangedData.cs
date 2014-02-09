using System.Runtime.Serialization;
using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.Heroes;
using Infrastructure.Data;
using Infrastructure.DDDES;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Heroes
{
    [ProtoContract]
    public class StateChangedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<Hero> HeroId { get; private set; }

        [ProtoMember(2)]
        public HeroStateData State { get; private set; }

        private StateChangedData()//Protobuf
        {
        }

        public StateChangedData(IdentityOf<Hero> heroId, HeroStateData state)
        {
            HeroId = heroId;
            State = state;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}