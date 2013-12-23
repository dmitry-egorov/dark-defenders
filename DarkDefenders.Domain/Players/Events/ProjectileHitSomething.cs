using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Players.Entities.Projectiles;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Players.Events
{
    public class ProjectileHitSomething : EventBase<PlayerId, ProjectileHitSomething>, IPlayerEvent
    {
        public ProjectileId ProjectileId { get; private set; }

        public ProjectileHitSomething(PlayerId rootId, ProjectileId projectileId) : base(rootId)
        {
            ProjectileId = projectileId;
        }

        protected override string ToStringInternal()
        {
            return "Projectile hit something: {0}, {1}".FormatWith(RootId, ProjectileId);
        }

        protected override bool EventEquals(ProjectileHitSomething other)
        {
            return ProjectileId.Equals(other.ProjectileId);
        }

        protected override int GetEventHashCode()
        {
            return ProjectileId.GetHashCode();
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