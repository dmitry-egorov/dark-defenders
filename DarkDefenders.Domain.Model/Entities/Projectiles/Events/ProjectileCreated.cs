using DarkDefenders.Domain.Model.Entities.RigidBodies;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Projectiles.Events
{
    internal class ProjectileCreated : Created<Projectile>
    {
        private readonly RigidBody _rigidBody;

        public ProjectileCreated(IStorage<Projectile> storage, Projectile projectile, RigidBody rigidBody) : base(projectile, storage)
        {
            _rigidBody = rigidBody;
        }

        protected override void ApplyTo(Projectile entity)
        {
            entity.Created(_rigidBody);
        }

        public override void Accept(IEventsReciever reciever)
        {
            reciever.ProjectileCreated(_rigidBody.Id);
        }
    }
}