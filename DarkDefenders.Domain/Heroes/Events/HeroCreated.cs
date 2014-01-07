using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Terrains;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Heroes.Events
{
    public class HeroCreated : EventBase<HeroId, HeroCreated>, IDomainEvent
    {
        public CreatureId CreatureId { get; private set; }
        public TerrainId TerrainId { get; private set; }

        public HeroCreated(HeroId rootId, TerrainId terrainId, CreatureId creatureId)
            : base(rootId)
        {
            TerrainId = terrainId;
            CreatureId = creatureId;
        }

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}