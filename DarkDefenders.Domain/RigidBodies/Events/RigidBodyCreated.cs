using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Terrains;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies.Events
{
    public class RigidBodyCreated : EventBase<RigidBodyId, RigidBodyCreated>, IDomainEvent
    {
        public ClockId ClockId { get; private set; }
        public TerrainId TerrainId { get; private set; }
        public Vector Position { get; private set; }
        public Momentum Momentum { get; private set; }
        public RigidBodyProperties Properties { get; private set; }

        public RigidBodyCreated(RigidBodyId rigidBodyId, ClockId clockId, TerrainId terrainId, Vector position, Momentum momentum, RigidBodyProperties properties)
            : base(rigidBodyId)
        {
            ClockId = clockId.ShouldNotBeNull("clockId");
            TerrainId = terrainId.ShouldNotBeNull("terrainId");
            Position = position;
            Momentum = momentum;
            Properties = properties;
        }
        
        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}