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
        public Circle BoundingCircle { get; private set; }
        public double Mass { get; private set; }
        public Vector InitialMomentum { get; private set; }

        public RigidBodyCreated(RigidBodyId rigidBodyId, WorldId worldId, Circle boundingCircle, Vector initialMomentum, double mass)
            : base(rigidBodyId)
        {
            WorldId = worldId.ShouldNotBeNull("worldId");
            BoundingCircle = boundingCircle;
            Mass = mass;
            InitialMomentum = initialMomentum;
        }
        
        protected override string ToStringInternal()
        {
            return "RigidBody created {0}, {1}, {2}, {3}, {4}".FormatWith(RootId, WorldId, BoundingCircle, Mass, InitialMomentum);
        }

        protected override bool EventEquals(RigidBodyCreated other)
        {
            return WorldId.Equals(other.WorldId) 
                && BoundingCircle.Equals(other.BoundingCircle)
                && Mass.Equals(other.Mass)
                && InitialMomentum.Equals(other.InitialMomentum);
        }

        protected override int GetEventHashCode()
        {
            unchecked
            {
                var hashCode = WorldId.GetHashCode();
                hashCode = (hashCode * 397) ^ BoundingCircle.GetHashCode();
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