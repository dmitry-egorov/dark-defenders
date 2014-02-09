using System.Runtime.Serialization;
using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Data.Other;
using DarkDefenders.Domain.Entities.Creatures;
using Infrastructure.Data;
using Infrastructure.DDDES;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Creatures
{
    [ProtoContract]
    public class MovementChangedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<Creature> CreatureId { get; private set; }

        [ProtoMember(2)]
        public Movement Movement { get; private set; }

        private MovementChangedData()//Protobuf
        {
        }

        public MovementChangedData(IdentityOf<Creature> creatureId, Movement movement)
        {
            CreatureId = creatureId;
            Movement = movement;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}