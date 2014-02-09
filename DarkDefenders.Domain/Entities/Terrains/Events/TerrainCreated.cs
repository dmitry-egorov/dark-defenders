using DarkDefenders.Domain.Data.Entities.Terrains;
using DarkDefenders.Domain.Data.Other;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Entities.Terrains.Events
{
    internal class TerrainCreated : Created<Terrain>
    {
        private readonly Map<Tile> _map;
        private readonly string _mapId;

        public TerrainCreated(IStorage<Terrain> storage, Map<Tile> map, string mapId) : base(storage)
        {
            _map = map;
            _mapId = mapId;
        }

        protected override Terrain Create()
        {
            return new Terrain(_map);
        }

        protected override object CreateData(IdentityOf<Terrain> id)
        {
            return new TerrainCreatedData(id, _mapId);
        }
    }
}