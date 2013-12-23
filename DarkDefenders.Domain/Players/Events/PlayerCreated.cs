using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Players.Events
{
    public class PlayerCreated : EventBase<PlayerId, PlayerCreated>, IDomainEvent
    {
        public WorldId WorldId { get; private set; }
        public RigidBodyId RigidBodyId { get; private set; }

        public PlayerCreated(PlayerId playerId, WorldId worldId, RigidBodyId rigidBodyId) 
            : base(playerId)
        {
            WorldId = worldId;
            RigidBodyId = rigidBodyId;
        }

        protected override string ToStringInternal()
        {
            return "Player created {0}, {1}, {2}".FormatWith(RootId, WorldId, RigidBodyId);
        }

        protected override bool EventEquals(PlayerCreated other)
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