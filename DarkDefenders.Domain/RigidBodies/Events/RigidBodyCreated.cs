using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies.Events
{
    public class RigidBodyCreated : EventBase<RigidBodyId, RigidBodyCreated>, IRigidBodyEvent
    {
        public WorldId WorldId { get; private set; }

        public Circle BoundingCircle { get; private set; }

        public RigidBodyCreated(RigidBodyId rigidBodyId, WorldId worldId, Circle boundingCircle) : base(rigidBodyId)
        {
            WorldId = worldId;

            BoundingCircle = boundingCircle;
        }

        public void ApplyTo(IRigidBodyEventsReciever reciever)
        {
            reciever.Apply(this);
        }

        protected override string ToStringInternal()
        {
            return "RigidBody created {0}, {1}, {2}".FormatWith(RootId, WorldId, BoundingCircle);
        }

        protected override bool EventEquals(RigidBodyCreated other)
        {
            return WorldId.Equals(other.WorldId) && BoundingCircle.Equals(other.BoundingCircle);
        }

        protected override int GetEventHashCode()
        {
            return (WorldId.GetHashCode() * 397) ^ BoundingCircle.GetHashCode();
        }
    }
}