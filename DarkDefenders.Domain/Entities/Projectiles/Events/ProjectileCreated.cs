using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Dtos.Entities.Projectiles;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Projectiles.Events
{
    internal class ProjectileCreated : Created<Projectile, ProjectileId>
    {
        private readonly IStorage<Projectile> _storage;
        private readonly IContainer<RigidBody> _rigidBodyContainer;

        public ProjectileCreated(IStorage<Projectile> storage, IContainer<RigidBody> rigidBodyContainer) : base(storage)
        {
            _storage = storage;
            _rigidBodyContainer = rigidBodyContainer;
        }


        protected override object CreateDto(ProjectileId projectileId)
        {
            var rigidBodyId = _rigidBodyContainer.Item.GetGlobalId();

            return new ProjectileCreatedDto(projectileId, rigidBodyId);
        }

        protected override Projectile Create()
        {
            var rigidBody = _rigidBodyContainer.Item;

            return new Projectile(_storage, rigidBody);
        }
    }
}