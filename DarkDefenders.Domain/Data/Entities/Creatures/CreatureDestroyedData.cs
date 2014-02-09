using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.Creatures;
using Infrastructure.Data;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Creatures
{
    [ProtoContract]
    public class CreatureDestroyedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<Creature> CreatureId { get; private set; }

        private CreatureDestroyedData()//Protobuf
        {
        }

        public CreatureDestroyedData(IdentityOf<Creature> creatureId)
        {
            CreatureId = creatureId;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}