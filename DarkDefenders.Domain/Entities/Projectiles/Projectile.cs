using System.Collections.Generic;
using DarkDefenders.Domain.Entities.Projectiles.Events;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Dtos.Entities.Projectiles;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Projectiles
{
    internal class Projectile : Entity<ProjectileId>
    {
        public const double Mass = 0.001;
        public const double BoundingBoxRadius = 0.1;

        private readonly RigidBody _rigidBody;
        private readonly IStorage<Projectile> _storage;

        public Projectile(IStorage<Projectile> storage, RigidBody rigidBody)
        {
            _storage = storage;
            _rigidBody = rigidBody;
        }

        public IEnumerable<IEvent> CheckForHit()
        {
            if (!IsHit())
            {
                yield break;
            }

            yield return new ProjectileDestroyed(this, _storage);

            var events = _rigidBody.Destroy();

            foreach (var e in events) { yield return e; }
        }

        private bool IsHit()
        {
            return _rigidBody.IsTouchingAnyWalls();
        }
    }
}