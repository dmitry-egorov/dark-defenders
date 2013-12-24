using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.RigidBodies;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Projectiles.Events
{
    public class ProjectileCreated : EventBase<ProjectileId, ProjectileCreated>, IDomainEvent
    {
        public RigidBodyId RigidBodyId { get; private set; }

        public ProjectileCreated(ProjectileId id, RigidBodyId rigidBodyId)
            : base(id)
        {
            RigidBodyId = rigidBodyId;
        }

        protected override string ToStringInternal()
        {
            return "Player's projectile created: {0}, {1}".FormatWith(RootId, RigidBodyId);
        }

        protected override bool EventEquals(ProjectileCreated other)
        {
            return RigidBodyId.Equals(other.RigidBodyId);
        }

        protected override int GetEventHashCode()
        {
            return RigidBodyId.GetHashCode();
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}