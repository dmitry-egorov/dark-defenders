using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Projectiles.Events
{
    internal class ProjectileCreated : Created<Projectile>
    {
        private readonly IStorage<Projectile> _storage;
        private readonly IContainer<RigidBody> _rigidBody;

        public ProjectileCreated(IStorage<Projectile> storage, IContainer<RigidBody> rigidBody) : base(storage)
        {
            _storage = storage;
            _rigidBody = rigidBody;
        }

        protected override Projectile Create()
        {
            return new Projectile(_storage, _rigidBody.Entity);
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Projectile> id)
        {
            reciever.ProjectileCreated(_rigidBody.Entity.Id);
        }
    }
}