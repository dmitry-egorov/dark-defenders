using DarkDefenders.Domain.Data.Other;
using DarkDefenders.Domain.Infrastructure;

using Infrastructure.DDDES;
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

        protected override void Accept(IEventsReciever reciever, IdentityOf<Terrain> id)
        {
            reciever.TerrainCreated(_mapId);
        }
    }
}