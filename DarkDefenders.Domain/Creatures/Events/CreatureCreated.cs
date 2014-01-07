using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Terrains;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Creatures.Events
{
    public class CreatureCreated : EventBase<CreatureId, CreatureCreated>, IDomainEvent
    {
        public ClockId ClockId { get; private set; }
        public TerrainId TerrainId { get; private set; }
        public RigidBodyId RigidBodyId { get; private set; }
        public CreatureProperties Properties { get; private set; }

        public CreatureCreated(CreatureId creatureId, ClockId clockId, TerrainId terrainId, RigidBodyId rigidBodyId, CreatureProperties properties) 
            : base(creatureId)
        {
            ClockId = clockId;
            Properties = properties;
            TerrainId = terrainId;
            RigidBodyId = rigidBodyId;
        }

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}