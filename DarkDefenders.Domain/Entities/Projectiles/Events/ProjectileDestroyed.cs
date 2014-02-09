using DarkDefenders.Domain.Data.Entities.Projectiles;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Projectiles.Events
{
    internal class ProjectileDestroyed : Destroyed<Projectile>
    {
        public ProjectileDestroyed(Projectile root, IStorage<Projectile> storage) 
            : base(root, storage)
        {
        }

        protected override object CreateData(IdentityOf<Projectile> id)
        {
            return new ProjectileDestroyedData(id);
        }
    }
}