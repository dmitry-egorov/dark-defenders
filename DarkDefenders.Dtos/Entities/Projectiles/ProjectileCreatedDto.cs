using DarkDefenders.Dtos.Entities.RigidBodies;
using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.Projectiles
{
    public class ProjectileCreatedDto : EventDto<ProjectileCreatedDto>
    {
        public ProjectileId ProjectileId { get; private set; }
        public RigidBodyId RigidBodyId { get; private set; }

        public ProjectileCreatedDto(ProjectileId projectileId, RigidBodyId rigidBodyId)
        {
            ProjectileId = projectileId;
            RigidBodyId = rigidBodyId;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}