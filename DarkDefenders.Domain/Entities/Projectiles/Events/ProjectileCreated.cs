using DarkDefenders.Domain.Data.Entities.Projectiles;
using DarkDefenders.Domain.Entities.RigidBodies;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

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
            return new Projectile(_storage, _rigidBody.Item);
        }

        protected override object CreateData(IdentityOf<Projectile> id)
        {
            var rigidBodyId = _rigidBody.Item.Id;

            return new ProjectileCreatedData(id, rigidBodyId);
        }
    }
}