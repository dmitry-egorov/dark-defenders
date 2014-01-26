using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Dtos.Entities.Projectiles;
using DarkDefenders.Dtos.Infrastructure;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Projectiles.Events
{
    internal class ProjectileDestroyed : Destroyed<Projectile, ProjectileId>
    {
        public ProjectileDestroyed(Projectile root, IStorage<Projectile> storage) 
            : base(root, storage)
        {
        }

        protected override IEventDto CreateDto(ProjectileId id)
        {
            return new ProjectileDestroyedDto(id);
        }
    }
}