using DarkDefenders.Domain.Entities.Other;
using DarkDefenders.Dtos.Entities.Terrains;
using DarkDefenders.Dtos.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Entities.Terrains.Events
{
    internal class TerrainCreated : Created<Terrain, TerrainId>
    {
        private readonly Map<Tile> _map;

        public TerrainCreated(IStorage<Terrain> storage, Map<Tile> map) : base(storage)
        {
            _map = map;
        }

        protected override object CreateDto(TerrainId rootId)
        {
            return new TerrainCreatedDto(rootId, _map);
        }

        protected override Terrain Create()
        {
            return new Terrain(_map);
        }
    }
}