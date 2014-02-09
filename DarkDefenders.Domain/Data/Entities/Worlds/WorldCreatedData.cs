using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.Worlds;
using Infrastructure.Data;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Worlds
{
    [ProtoContract]
    public class WorldCreatedData : EventDataBase
    {
        [ProtoMember(1)]
        public IdentityOf<World> WorldId { get; private set; }

        [ProtoMember(2)]
        public WorldProperties WorldProperties { get; private set; }

        private WorldCreatedData()//Protobuf
        {
        }

        public WorldCreatedData(IdentityOf<World> worldId, WorldProperties worldProperties)
        {
            WorldId = worldId;
            WorldProperties = worldProperties;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}