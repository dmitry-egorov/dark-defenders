using System.Collections.Generic;
using DarkDefenders.Domain.Data.Other;
using DarkDefenders.Domain.Entities.Terrains;
using DarkDefenders.Domain.Entities.Terrains.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Factories
{
    internal class TerrainFactory: Factory<Terrain>
    {
        public TerrainFactory(IStorage<Terrain> storage)
            : base(storage)
        {
        }

        public ICreation<Terrain> Create(Map<Tile> map, string mapId)
        {
            return GetCreation(s => Create(s, map, mapId));
        }

        private static IEnumerable<IEvent> Create(IStorage<Terrain> storage, Map<Tile> map, string mapId)
        {
            yield return new TerrainCreated(storage, map, mapId);
        }
    }
}