using DarkDefenders.Domain.Infrastructure;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Projectiles.Events
{
    internal class ProjectileDestroyed : Destroyed<Projectile>
    {
        public ProjectileDestroyed(Projectile root, IStorage<Projectile> storage) 
            : base(root, storage)
        {
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Projectile> id)
        {
        }
    }
}