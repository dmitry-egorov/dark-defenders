using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.Heroes;
using Infrastructure.Data;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Heroes
{
    [ProtoContract]
    public class HeroCreatedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<Hero> HeroId { get; private set; }

        [ProtoMember(2)]
        public IdentityOf<Creature> CreatureId { get; private set; }

        private HeroCreatedData()//Protobuf
        {
        }

        public HeroCreatedData(IdentityOf<Hero> heroId, IdentityOf<Creature> creatureId)
        {
            HeroId = heroId;
            CreatureId = creatureId;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}