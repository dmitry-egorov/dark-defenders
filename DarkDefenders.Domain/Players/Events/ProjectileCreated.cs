using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Players.Entities.Projectiles;
using DarkDefenders.Domain.RigidBodies;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Players.Events
{
    public class ProjectileCreated : EventBase<PlayerId, ProjectileCreated>, IPlayerEvent
    {
        public ProjectileId ProjectileId { get; private set; }
        public RigidBodyId RigidBodyId { get; private set; }
        public double Time { get; private set; }

        public ProjectileCreated(PlayerId playerId, ProjectileId projectileId, RigidBodyId rigidBodyId, double time)
            : base(playerId)
        {
            Time = time;
            ProjectileId = projectileId;
            RigidBodyId = rigidBodyId;
        }

        protected override string ToStringInternal()
        {
            return "Player's projectile created: {0}, {1}, {2}".FormatWith(RootId, ProjectileId, RigidBodyId);
        }

        protected override bool EventEquals(ProjectileCreated other)
        {
            return ProjectileId.Equals(other.ProjectileId) && RigidBodyId.Equals(other.RigidBodyId);
        }

        protected override int GetEventHashCode()
        {
            return (ProjectileId.GetHashCode() * 397) ^ RigidBodyId.GetHashCode();
        }

        public void ApplyTo(IPlayerEventsReciever reciever)
        {
            reciever.Recieve(this);
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}