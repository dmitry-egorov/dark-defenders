using DarkDefenders.Dtos.Infrastructure;

namespace DarkDefenders.Dtos.Entities.Projectiles
{
    public class ProjectileDestroyedDto : EventDto<ProjectileDestroyedDto>
    {
        public ProjectileId Id { get; private set; }

        public ProjectileDestroyedDto(ProjectileId id)
        {
            Id = id;
        }

        public override void Accept(IEventDtoReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}