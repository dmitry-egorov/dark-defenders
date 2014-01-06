using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Creatures.Events
{
    public class CreatureCreated : EventBase<CreatureId, CreatureCreated>, IDomainEvent
    {
        public ClockId ClockId { get; private set; }
        public WorldId WorldId { get; private set; }
        public RigidBodyId RigidBodyId { get; private set; }
        public CreatureProperties Properties { get; private set; }

        public CreatureCreated(CreatureId creatureId, ClockId clockId, WorldId worldId, RigidBodyId rigidBodyId, CreatureProperties properties) 
            : base(creatureId)
        {
            ClockId = clockId;
            Properties = properties;
            WorldId = worldId;
            RigidBodyId = rigidBodyId;
        }

        protected override string ToStringInternal()
        {
            return "Creature created {0}, {1}, {2}, {3}, {4}".FormatWith(RootId, ClockId, WorldId, RigidBodyId, Properties);
        }

        protected override bool EventEquals(CreatureCreated other)
        {
            return ClockId.Equals(other.ClockId)
                && WorldId.Equals(other.WorldId)
                && RigidBodyId.Equals(other.RigidBodyId)
                && Properties.Equals(other.Properties);
        }

        protected override int GetEventHashCode()
        {
            var hashCode = ClockId.GetHashCode();
            hashCode = (hashCode * 397) ^ WorldId.GetHashCode();
            hashCode = (hashCode * 397) ^ RigidBodyId.GetHashCode();
            hashCode = (hashCode * 397) ^ Properties.GetHashCode();
            return hashCode;
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}