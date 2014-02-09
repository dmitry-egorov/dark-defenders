using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.Worlds;
using Infrastructure.Data;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Worlds
{
    [ProtoContract]
    public class SpawnHeroesChangedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<World> WorldId { get; private set; }

        [ProtoMember(2)]
        public bool Enabled { get; private set; }

        private SpawnHeroesChangedData()//Protobuf
        {
        }

        public SpawnHeroesChangedData(IdentityOf<World> worldId, bool enabled)
        {
            WorldId = worldId;
            Enabled = enabled;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}