using DarkDefenders.Dtos.Infrastructure;
using DarkDefenders.Dtos.Other;
using Infrastructure.Math;

namespace DarkDefenders.Dtos.Entities.Terrains
{
    public class TerrainCreatedDto : EventDto<TerrainCreatedDto>
    {
        public TerrainId TerrainId { get; private set; }
        public Map<Tile> Map { get; private set; }

        public TerrainCreatedDto(TerrainId terrainId, Map<Tile> map)
        {
            TerrainId = terrainId;
            Map = map;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}