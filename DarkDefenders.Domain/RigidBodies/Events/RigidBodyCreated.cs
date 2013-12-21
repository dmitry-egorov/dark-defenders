using DarkDefenders.Domain.Players;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies.Events
{
    public class RigidBodyCreated : EventBase<RigidBodyId, RigidBodyCreated>, IRigidBodyEvent
    {
        public WorldId WorldId { get; private set; }

        public Vector SpawnPosition { get; private set; }

        public RigidBodyCreated(RigidBodyId rigidBodyId, WorldId worldId, Vector spawnPosition) : base(rigidBodyId)
        {
            WorldId = worldId;

            SpawnPosition = spawnPosition;
        }

        public void ApplyTo(IRigidBodyEventsReciever reciever)
        {
            reciever.Apply(this);
        }

        protected override string EventToString()
        {
            return "RigidBody created {0}, {1}, {2}".FormatWith(RootId, WorldId, SpawnPosition);
        }

        protected override bool EventEquals(RigidBodyCreated other)
        {
            return WorldId.Equals(other.WorldId) && SpawnPosition.Equals(other.SpawnPosition);
        }

        protected override int GetEventHashCode()
        {
            return (WorldId.GetHashCode() * 397) ^ SpawnPosition.GetHashCode();
        }
    }
}