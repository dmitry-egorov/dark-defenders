using System;
using System.Runtime.Serialization;
using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.Creatures;
using Infrastructure.Data;
using Infrastructure.DDDES;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Creatures
{
    [ProtoContract]
    public class FiredData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<Creature> CreatureId { get; private set; }

        [ProtoMember(2)]
        public TimeSpan Time { get; private set; }

        private FiredData()//Protobuf
        {
        }

        public FiredData(IdentityOf<Creature> creatureId, TimeSpan time)
        {
            CreatureId = creatureId;
            Time = time;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}