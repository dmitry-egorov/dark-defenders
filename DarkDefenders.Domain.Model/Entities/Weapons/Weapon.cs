using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Model.Entities.Clocks;
using DarkDefenders.Domain.Model.Entities.Projectiles;
using DarkDefenders.Domain.Model.Entities.RigidBodies;
using DarkDefenders.Domain.Model.Entities.Weapons.Events;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities.Weapons
{
    [UsedImplicitly]
    public class Weapon: Entity<Weapon>
    {
        private static readonly TimeSpan _fireDelay = TimeSpan.FromSeconds(0.25);

        private const double ProjectileMomentum = 150.0 * Projectile.Mass;

        private static readonly Momentum _leftProjectileMomentum = Vector.XY(-ProjectileMomentum, 0).ToMomentum();
        private static readonly Momentum _rightProjectileMomentum = Vector.XY(ProjectileMomentum, 0).ToMomentum();

        private readonly IFactory<Projectile> _projectileFactory;
        private readonly Clock _clock;
        private readonly Cooldown _fireCooldown;
        private readonly IStorage<Weapon> _storage;

        private RigidBody _rigidBody;

        public Weapon(IStorage<Weapon> storage, Clock clock, IFactory<Projectile> projectileFactory)
        {
            _clock = clock;
            _projectileFactory = projectileFactory;
            _storage = storage;
            _fireCooldown = new Cooldown(clock, _fireDelay);
        }

        public IEnumerable<IEvent> Create(RigidBody rigidBody)
        {
            yield return new WeaponCreated(this, _storage, rigidBody);
        }

        public IEnumerable<IEvent> Fire(Direction direction)
        {
            if (_fireCooldown.IsInEffect())
            {
                yield break;
            }

            var events = CreateProjectile(direction);

            foreach (var e in events) { yield return e; }

            var currentTime = _clock.GetCurrentTime();

            yield return new Fired(this, currentTime);
        }

        internal void Created(RigidBody rigidBody)
        {
            _rigidBody = rigidBody;
        }

        internal void Fired(TimeSpan activationTime)
        {
            _fireCooldown.SetLastActivationTime(activationTime);
        }

        private IEnumerable<IEvent> CreateProjectile(Direction direction)
        {
            var projectilePosition = GetProjectilePosition(direction);
            var projectileMomentum = GetProjectileMomentum(direction);

            var projectile = _projectileFactory.Create();

            var events = projectile.Create(projectilePosition, projectileMomentum);

            foreach (var e in events) { yield return e; }
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