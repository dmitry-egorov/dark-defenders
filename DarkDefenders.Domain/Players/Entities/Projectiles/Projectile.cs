using System.Collections.Generic;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.RigidBodies;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Players.Entities.Projectiles
{
    public class Projectile
    {
        private const double Momentum = 3.0 * Mass;
        public const double Mass = 0.001;
        public const double BoundingCircleRadius = 1.0 / 100.0;

        public static readonly Vector LeftMomentum = Vector.XY(-Momentum, 0);
        public static readonly Vector RightMomentum = Vector.XY(Momentum, 0);

        public Projectile(RigidBody rigidBody, PlayerId playerId, ProjectileId projectileId)
        {
            _rigidBody = rigidBody;
            _playerId = playerId;
            _projectileId = projectileId;
        }

        private readonly RigidBody _rigidBody;
        private readonly PlayerId _playerId;
        private readonly ProjectileId _projectileId;

        public IEnumerable<IEvent> CheckForHit()
        {
            if (!IsHit())
            {
                yield break;
            }

            yield return new ProjectileHitSomething(_playerId, _projectileId);

            foreach (var e in _rigidBody.Destroy()) { yield return e; }
        }

        private bool IsHit()
        {
            return _rigidBody.IsAdjacentToAWall();
        }
    }
}