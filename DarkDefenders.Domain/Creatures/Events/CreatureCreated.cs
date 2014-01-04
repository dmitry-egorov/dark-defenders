using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Creatures.Events
{
    public class CreatureCreated : EventBase<CreatureId, CreatureCreated>, IDomainEvent
    {
        public WorldId WorldId { get; private set; }
        public RigidBodyId RigidBodyId { get; private set; }

        public CreatureCreated(CreatureId creatureId, WorldId worldId, RigidBodyId rigidBodyId) 
            : base(creatureId)
        {
            WorldId = worldId;
            RigidBodyId = rigidBodyId;
        }

        protected override string ToStringInternal()
        {
            return "Creature created {0}, {1}, {2}".FormatWith(RootId, WorldId, RigidBodyId);
        }

        protected override bool EventEquals(CreatureCreated other)
        {
            return WorldId.Equals(other.WorldId) && RigidBodyId.Equals(other.RigidBodyId);
        }

        protected override int GetEventHashCode()
        {
            return (WorldId.GetHashCode() * 397) ^ RigidBodyId.GetHashCode();
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}