using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Math.Physics;
using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies.Events
{
    public class RigidBodyCreated : EventBase<RigidBodyId, RigidBodyCreated>, IDomainEvent
    {
        public WorldId WorldId { get; private set; }
        public Vector Position { get; private set; }
        public Momentum Momentum { get; private set; }
        public RigidBodyProperties Properties { get; private set; }

        public RigidBodyCreated(RigidBodyId rigidBodyId, WorldId worldId, Vector position, Momentum momentum, RigidBodyProperties properties)
            : base(rigidBodyId)
        {
            WorldId = worldId.ShouldNotBeNull("worldId");
            Position = position;
            Momentum = momentum;
            Properties = properties;
        }
        
        protected override string ToStringInternal()
        {
            return "RigidBody created {0}, {1}, {2}, {3}, {4}".FormatWith(RootId, WorldId, Position, Momentum, Properties);
        }

        protected override bool EventEquals(RigidBodyCreated other)
        {
            return WorldId.Equals(other.WorldId)
                && Position.Equals(other.Position)
                && Properties.Equals(other.Properties)
                && Momentum.Equals(other.Momentum);
        }

        protected override int GetEventHashCode()
        {
            unchecked
            {
                var hashCode = WorldId.GetHashCode();
                hashCode = (hashCode * 397) ^ Position.GetHashCode();
                hashCode = (hashCode * 397) ^ Properties.GetHashCode();
                hashCode = (hashCode * 397) ^ Momentum.GetHashCode();
                return hashCode;
            }
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}