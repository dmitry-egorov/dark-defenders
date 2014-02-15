using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Terrains.Events
{
    internal class TerrainCreated : Created<Terrain>
    {
        private readonly string _mapId;

        public TerrainCreated(Terrain terrain, IStorage<Terrain> storage, string mapId) : base(terrain, storage)
        {
            _mapId = mapId;
        }

        protected override void ApplyTo(Terrain terrain)
        {
            terrain.Created(_mapId);
        }

        public override void Accept(IEventsReciever reciever)
        {
            reciever.TerrainCreated(_mapId);
        }
    }
}