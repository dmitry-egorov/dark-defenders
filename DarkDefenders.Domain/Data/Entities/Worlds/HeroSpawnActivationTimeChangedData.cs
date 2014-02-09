using System;
using System.Runtime.Serialization;
using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.Worlds;
using Infrastructure.Data;
using Infrastructure.DDDES;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Worlds
{
    [ProtoContract]
    public class HeroSpawnActivationTimeChangedData : EventDataBase
    {
        [ProtoMember(2)]
        public TimeSpan Time { get; private set; }

        [ProtoMember(1)]
        public IdentityOf<World> WorldId { get; private set; }

        private HeroSpawnActivationTimeChangedData()//Protobuf
        {
        }

        public HeroSpawnActivationTimeChangedData(IdentityOf<World> worldId, TimeSpan time)
        {
            WorldId = worldId;
            Time = time;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}