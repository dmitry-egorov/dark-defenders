using System.Runtime.Serialization;
using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.Heroes;
using Infrastructure.Data;
using Infrastructure.DDDES;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Heroes
{
    [ProtoContract]
    public class HeroDestroyedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<Hero> HeroId { get; private set; }

        private HeroDestroyedData()//Protobuf
        {
        }

        public HeroDestroyedData(IdentityOf<Hero> heroId)
        {
            HeroId = heroId;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}