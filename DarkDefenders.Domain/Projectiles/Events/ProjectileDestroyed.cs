using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Projectiles.Events
{
    public class ProjectileDestroyed : Destroyed<ProjectileId, Projectile, ProjectileDestroyed>, IDomainEvent
    {
        public ProjectileDestroyed(ProjectileId rootId) : base(rootId)
        {
        }

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}