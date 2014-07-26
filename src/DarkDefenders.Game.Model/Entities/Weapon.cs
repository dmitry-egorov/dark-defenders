using System;
using DarkDefenders.Game.Model.Events;
using DarkDefenders.Game.Model.Other;
using DarkDefenders.Kernel.Model;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using JetBrains.Annotations;

namespace DarkDefenders.Game.Model.Entities
{
    [UsedImplicitly]
    public class Weapon : Entity<Weapon, IWeaponEvents>, IWeaponEvents
    {
        private static readonly TimeSpan _fireDelay = TimeSpan.FromSeconds(0.25);

        private const double ProjectileMomentum = 150.0 * Projectile.Mass;

        private static readonly Momentum _leftProjectileMomentum = Vector.XY(-ProjectileMomentum, 0).ToMomentum();
        private static readonly Momentum _rightProjectileMomentum = Vector.XY(ProjectileMomentum, 0).ToMomentum();

        private readonly IFactory<Projectile> _projectileFactory;
        private readonly Cooldown _fireCooldown;

        private RigidBody _rigidBody;

        public Weapon(IFactory<Projectile> projectileFactory, Cooldown fireCooldown)
        {
            _projectileFactory = projectileFactory;
            _fireCooldown = fireCooldown;
        }

        public void Create(RigidBody rigidBody)
        {
            _fireCooldown.Create(_fireDelay);
            CreationEvent(x => x.Created(rigidBody));
        }

        public void TryFire(Direction direction)
        {
            _fireCooldown.TryActivate(() => CreateProjectile(direction));
        }

        void IWeaponEvents.Created(RigidBody rigidBody)
        {
            _rigidBody = rigidBody;
        }

        private void CreateProjectile(Direction direction)
        {
            var projectilePosition = GetProjectilePosition(direction);
            var projectileMomentum = GetProjectileMomentum(direction);

            _projectileFactory.Create().Create(projectilePosition, projectileMomentum);
        }

        private Vector GetProjectilePosition(Direction direction)
        {
            var offset =  GetProjectileOffset(direction);

            var position = _rigidBody.GetPosition();

            return position.OffsetBy(Axis.Horizontal, offset);
        }

        private static double GetProjectileOffset(Direction direction)
        {
            const double radius = 1.0;
            return 
            direction == Direction.Right 
            ? radius
            : -radius;
        }

        private static Momentum GetProjectileMomentum(Direction direction)
        {
            return 
            direction == Direction.Right
            ? _rightProjectileMomentum
            : _leftProjectileMomentum;
        }
    }
}