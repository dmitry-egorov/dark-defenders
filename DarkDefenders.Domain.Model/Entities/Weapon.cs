using System;
using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities
{
    [UsedImplicitly]
    public class Weapon : Entity<Weapon, IWeaponEvents>, IWeaponEvents
    {
        private static readonly TimeSpan _fireDelay = TimeSpan.FromSeconds(0.25);

        private const double ProjectileMomentum = 150.0 * Projectile.Mass;

        private static readonly Momentum _leftProjectileMomentum = Vector.XY(-ProjectileMomentum, 0).ToMomentum();
        private static readonly Momentum _rightProjectileMomentum = Vector.XY(ProjectileMomentum, 0).ToMomentum();

        private readonly IFactory<Projectile> _projectileFactory;
        private readonly Clock _clock;
        private readonly Cooldown _fireCooldown;

        private RigidBody _rigidBody;

        public Weapon(Clock clock, IFactory<Projectile> projectileFactory)
        {
            _clock = clock;
            _projectileFactory = projectileFactory;
            _fireCooldown = new Cooldown(clock, _fireDelay);
        }

        public void Create(RigidBody rigidBody)
        {
            CreationEvent(x => x.Created(rigidBody));
        }

        public void Fire(Direction direction)
        {
            if (_fireCooldown.IsInEffect())
            {
                return;
            }

            CreateProjectile(direction);

            var currentTime = _clock.GetCurrentTime();

            Event(x => x.Fired(currentTime));
        }

        void IWeaponEvents.Created(RigidBody rigidBody)
        {
            _rigidBody = rigidBody;
        }

        void IWeaponEvents.Fired(TimeSpan activationTime)
        {
            _fireCooldown.SetLastActivationTime(activationTime);
        }

        private void CreateProjectile(Direction direction)
        {
            var projectilePosition = GetProjectilePosition(direction);
            var projectileMomentum = GetProjectileMomentum(direction);

            var projectile = _projectileFactory.Create();

            projectile.Create(projectilePosition, projectileMomentum);
        }

        private Vector GetProjectilePosition(Direction direction)
        {
            var position = _rigidBody.Position;
            var x = position.X;
            var y = position.Y;

            const double radius = 1.0;

            if (direction == Direction.Right)
            {
                x += radius;
            }
            else
            {
                x -= radius;
            }

            return Vector.XY(x, y);
        }

        private static Momentum GetProjectileMomentum(Direction direction)
        {
            return direction == Direction.Right
                   ? _rightProjectileMomentum
                   : _leftProjectileMomentum;
        }
    }
}