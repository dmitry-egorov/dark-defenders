using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies.Events
{
    public class RigidBodyCreated : EventBase<RigidBodyId, RigidBodyCreated>, IDomainEvent
    {
        public WorldId WorldId { get; private set; }
        public Box BoundingBox { get; private set; }
        public double Mass { get; private set; }
        public Vector InitialMomentum { get; private set; }
        public double TopHorizontalMomentum { get; private set; }

        public RigidBodyCreated(RigidBodyId rigidBodyId, WorldId worldId, Box boundingBox, Vector initialMomentum, double mass, double topHorizontalMomentum)
            : base(rigidBodyId)
        {
            WorldId = worldId.ShouldNotBeNull("worldId");
            BoundingBox = boundingBox;
            Mass = mass;
            TopHorizontalMomentum = topHorizontalMomentum;
            InitialMomentum = initialMomentum;
        }
        
        protected override string ToStringInternal()
        {
            return "RigidBody created {0}, {1}, {2}, {3}, {4}".FormatWith(RootId, WorldId, BoundingBox, Mass, InitialMomentum);
        }

        protected override bool EventEquals(RigidBodyCreated other)
        {
            return WorldId.Equals(other.WorldId) 
                && BoundingBox.Equals(other.BoundingBox)
                && Mass.Equals(other.Mass)
                && InitialMomentum.Equals(other.InitialMomentum);
        }

        protected override int GetEventHashCode()
        {
            unchecked
            {
                var hashCode = WorldId.GetHashCode();
                hashCode = (hashCode * 397) ^ BoundingBox.GetHashCode();
                hashCode = (hashCode * 397) ^ Mass.GetHashCode();
                hashCode = (hashCode * 397) ^ InitialMomentum.GetHashCode();
                return hashCode;
            }
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}