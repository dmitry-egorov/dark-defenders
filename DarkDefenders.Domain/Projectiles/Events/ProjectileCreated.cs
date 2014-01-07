using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.RigidBodies;
using Infrastructure.DDDES.Implementations.Domain;

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

        public void ApplyTo(IDomainEventsReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}