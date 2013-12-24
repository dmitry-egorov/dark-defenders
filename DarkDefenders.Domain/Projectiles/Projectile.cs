using System.Collections.Generic;
using DarkDefenders.Domain.Projectiles.Events;
using DarkDefenders.Domain.RigidBodies;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Projectiles
{
    public class Projectile: RootBase<ProjectileId, IProjectileEventsReciever, IProjectileEvent>, IProjectileEventsReciever
    {
        public const double Mass = 0.001;
        public const double BoundingCircleRadius = 1.0 / 100.0;

        public static readonly Vector LeftMomentum = Vector.XY(-Momentum, 0);
        public static readonly Vector RightMomentum = Vector.XY(Momentum, 0);

        public IEnumerable<IEvent> CheckForHit()
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

        private const double Momentum = 3.0 * Mass;

        private readonly RigidBody _rigidBody;
    }
}