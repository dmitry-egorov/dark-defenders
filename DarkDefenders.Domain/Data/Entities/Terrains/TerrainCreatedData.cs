using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Entities.Terrains;
using Infrastructure.Data;
using ProtoBuf;

namespace DarkDefenders.Domain.Data.Entities.Terrains
{
    [ProtoContract]
    public class TerrainCreatedData : EventDataBase
    {
        [ProtoMember(2)]
        public string MapId { get; private set; }

        [ProtoMember(1)]
        public IdentityOf<Terrain> TerrainId { get; private set; }


        private TerrainCreatedData()//Protobuf
        {
        }

        public TerrainCreatedData(IdentityOf<Terrain> terrainId, string mapId)
        {
            TerrainId = terrainId;
            MapId = mapId;
        }

        public override void Accept(IEventDataReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}