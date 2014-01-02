using System.Collections.Generic;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Projectiles.Events;
using DarkDefenders.Domain.RigidBodies;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Projectiles
{
    public class Projectile: RootBase<ProjectileId, IProjectileEventsReciever, IProjectileEvent>, IProjectileEventsReciever
    {
        public const double Mass = 0.001;
        public const double BoundingBoxRadius = 0.2;

        public IEnumerable<IDomainEvent> CheckForHit()
        {
            if (!IsHit())
            {
                yield break;
            }

            yield return new ProjectileDestroyed(Id);

            var events = _rigidBody.Destroy();

            foreach (var e in events) { yield return e; }
        }

        internal Projectile(ProjectileId id, RigidBody rigidBody) : base(id)
        {
            _rigidBody = rigidBody;
        }

        private bool IsHit()
        {
            return _rigidBody.IsAdjacentToAWall();
        }

        private readonly RigidBody _rigidBody;
    }
}