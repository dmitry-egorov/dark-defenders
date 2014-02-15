using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Projectiles.Events
{
    internal class ProjectileDestroyed : Destroyed<Projectile>
    {
        public ProjectileDestroyed(Projectile entity, IStorage<Projectile> storage) 
            : base(entity, storage)
        {
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Projectile> id)
        {
        }
    }
}