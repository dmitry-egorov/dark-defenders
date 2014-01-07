using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Other;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Terrains.Events
{
    public class TerrainCreated : EventBase<TerrainId, TerrainCreated>, IDomainEvent
    {
        public Map<Tile> Map { get; private set; }

        public TerrainCreated(TerrainId rootId, Map<Tile> map) : base(rootId)
        {
            Map = map;
        }


        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}