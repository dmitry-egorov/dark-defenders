using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Creatures.Events;
using DarkDefenders.Domain.Projectiles;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Math.Physics;

namespace DarkDefenders.Domain.Creatures
{
    public class Creature : RootBase<CreatureId, ICreatureEventsReciever, ICreatureEvent>, ICreatureEventsReciever
    {
        private const double MovementForce = 180.0;
        private const double JumpMomentum = 60;

        private static readonly Seconds _fireDelay = 0.25.ToSeconds();
        private const double ProjectileMomentum = 150.0 * Projectile.Mass;

        private const Direction InitialDirection = Direction.Right;

        public IEnumerable<IDomainEvent> SetMovement(Movement movement)
        {
            if (_movement == movement)
            {
                yield break;
            }

            yield return new MovementChanged(Id, movement);

            var movementForce = GetMovementForce(movement);

            var events = _rigidBody.SetExternalForce(movementForce);

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IDomainEvent> Jump()
        {
            if (CantJump())
            {
                yield break;
            }

            var events = AddJumpMomentum();

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IDomainEvent> Fire()
        {
            if (_fireCooldown.IsInEffect())
            {
                yield break;
            }
            
            var events = CreateProjectile();

            foreach (var e in events) { yield return e; }

            yield return new CreatureFired(Id, _world.GetCurrentTime());
        }

        public void Recieve(MovementChanged movementChanged)
        {
            _movement = movementChanged.Movement;
            _direction = GetDirection();
            _projectileMomentum = GetProjectileMomentum();
        }

        public void Recieve(CreatureFired creatureFired)
        {
            _fireCooldown.Activate(creatureFired.Time);
        }

        internal Creature(CreatureId id, ProjectileFactory projectileFactory, World world, RigidBody rigidBody) 
            : base(id)
        {
            _world = world;
            _rigidBody = rigidBody;
            _projectileFactory = projectileFactory;

            _movement = Movement.Stop;
            _direction = InitialDirection;
            _projectileMomentum = GetProjectileMomentum();

            _fireCooldown = new Cooldown(world, _fireDelay);
        }

        private bool CantJump()
        {
            return _rigidBody.IsInTheAir() || _rigidBody.HasVerticalMomentum();
        }

        private IEnumerable<IDomainEvent> AddJumpMomentum()
        {
            return _rigidBody.AddMomentum(_jumpMomentum);
        }

        private Direction GetDirection()
        {
            switch (_movement)
            {
                case Movement.Left:
                    return Direction.Left;
                case Movement.Right:
                    return Direction.Right;
                default:
                    return _direction;
            }
        }

        private Force GetMovementForce(Movement desiredMovement)
        {
            var force = GetMovementForceDirection(desiredMovement);

            if (_rigidBody.IsInTheAir())
            {
                force *= 0.5;
            }
            else if (_rigidBody.MomentumHasDifferentHorizontalDirectionFrom(force.Value))
            {
                force *= 2.0;
            }

            return force;
        }

        private static Force GetMovementForceDirection(Movement desiredMovement)
        {
            switch (desiredMovement)
            {
                case Movement.Stop:
                    return Force.Zero;
                case Movement.Left:
                    return _leftMovementForce;
                case Movement.Right:
                    return _rightMovementForce;
                default:
                    throw new ArgumentOutOfRangeException("desiredMovement");
            }
        }

        private IEnumerable<IDomainEvent> CreateProjectile()
        {
            var projectilePosition = GetProjectilePosition();

            var projectileId = new ProjectileId();

            return _projectileFactory.Create(projectileId, _world.Id, projectilePosition, _projectileMomentum);
        }

        private Momentum GetProjectileMomentum()
        {
            return _direction == Direction.Right 
                   ? _rightProjectileMomentum 
                   : _leftProjectileMomentum;
        }

        private Vector GetProjectilePosition()
        {
            var position = _rigidBody.Position;
            var x = position.X;
            var y = position.Y;

            const double radius = 1.0;

            if (_direction == Direction.Right)
            {
                x += radius;
            }
            else
            {
                x -= radius;
            }

            return Vector.XY(x, y);
        }

        private static readonly Momentum _jumpMomentum = Vector.XY(0, JumpMomentum).ToMomentum();
        private static readonly Force _leftMovementForce = Force.Left * MovementForce;
        private static readonly Force _rightMovementForce = Force.Right * MovementForce;

        private static readonly Momentum _leftProjectileMomentum = Vector.XY(-ProjectileMomentum, 0).ToMomentum();
        private static readonly Momentum _rightProjectileMomentum = Vector.XY(ProjectileMomentum, 0).ToMomentum();

        private readonly ProjectileFactory _projectileFactory;
        private readonly World _world;
        private readonly RigidBody _rigidBody;
        private readonly Cooldown _fireCooldown;

        private Movement _movement;
        private Direction _direction;
        private Momentum _projectileMomentum;
    }
}