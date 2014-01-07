using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Terrains.Events
{
    public class TerrainDestroyed : Destroyed<TerrainId, Terrain, TerrainDestroyed>, IDomainEvent
    {
        public TerrainDestroyed(TerrainId rootId)
            : base(rootId)
        {
        }

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}