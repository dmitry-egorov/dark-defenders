using System.Collections.Generic;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Terrains.Events;
using DarkDefenders.Domain.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Terrains
{
    public class TerrainFactory: RootFactory<TerrainId, Terrain, TerrainCreated>
    {
        public TerrainFactory(IRepository<TerrainId, Terrain> repository) : base(repository)
        {
        }

        public IEnumerable<IDomainEvent> Create(TerrainId terrainId, Map<Tile> map)
        {
            AssertDoesntExist(terrainId);

            yield return new TerrainCreated(terrainId, map);
        }

        protected override Terrain Handle(TerrainCreated creationEvent)
        {
            return new Terrain(creationEvent.RootId, creationEvent.Map);
        }
    }
}