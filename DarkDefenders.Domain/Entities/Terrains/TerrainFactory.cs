using System.Collections.Generic;
using DarkDefenders.Domain.Entities.Other;
using DarkDefenders.Domain.Entities.Terrains.Events;
using DarkDefenders.Dtos.Other;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Entities.Terrains
{
    internal class TerrainFactory
    {
        private readonly IStorage<Terrain> _storage;

        public TerrainFactory(IStorage<Terrain> storage)
        {
            _storage = storage;
        }

        public IEnumerable<IEvent> Create(Map<Tile> map)
        {
            yield return new TerrainCreated(_storage, map);
        }
    }
}