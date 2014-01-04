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

namespace DarkDefenders.Domain.Creatures
{
    public class Creature : RootBase<CreatureId, ICreatureEventsReciever, ICreatureEvent>, ICreatureEventsReciever
    {
        public const double BoundingBoxRadius = 0.4;
        public const double Mass = 1.0;
        public const double TopHorizontalMomentum = 40.0;

        private const double MovementForce = 180.0;
        private const double JumpMomentum = 60;

        private const double FireDelay = 0.25;
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
            if (FireDelayInEffect())
            {
                yield break;
            }
            
            var events = CreateProjectile();

            foreach (var e in events) { yield return e; }

            yield return new CreatureFired(Id, _world.TimeSeconds);
        }

        public void Recieve(MovementChanged movementChanged)
        {
            _movement = movementChanged.Movement;
            _direction = GetDirection();
            _projectileMomentum = GetProjectileMomentum();
        }

        public void Recieve(CreatureFired creatureFired)
        {
            _lastFireTime = creatureFired.Time;
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
        }

        private bool CantJump()
        {
            return _rigidBody.IsInTheAir() || _rigidBody.HasVerticalMomentum();
        }

        private IEnumerable<IDomainEvent> AddJumpMomentum()
        {
            return _rigidBody.AddMomentum(_jumpMomentum);
        }

        private bool FireDelayInEffect()
        {
            return _world.TimeSeconds - _lastFireTime < FireDelay;
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

        private Vector GetMovementForce(Movement desiredMovement)
        {
            var force = GetMovementForceDirection(desiredMovement);

            if (_rigidBody.IsInTheAir())
            {
                force *= 0.5;
            }
            else if (_rigidBody.MomentumHasDifferentHorizontalDirectionFrom(force))
            {
                force *= 2.0;
            }

            return force;
        }

        private static Vector GetMovementForceDirection(Movement desiredMovement)
        {
            switch (desiredMovement)
            {
                case Movement.Stop:
                    return Vector.Zero;
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

        private Vector GetProjectileMomentum()
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

            const double radius = BoundingBoxRadius + Projectile.BoundingBoxRadius;

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

        private static readonly Vector _jumpMomentum = Vector.XY(0, JumpMomentum);
        private static readonly Vector _leftMovementForce = Vector.Left * MovementForce;
        private static readonly Vector _rightMovementForce = Vector.Right * MovementForce;

        private static readonly Vector _leftProjectileMomentum = Vector.XY(-ProjectileMomentum, 0);
        private static readonly Vector _rightProjectileMomentum = Vector.XY(ProjectileMomentum, 0);

        private readonly ProjectileFactory _projectileFactory;
        private readonly World _world;
        private readonly RigidBody _rigidBody;

        private Movement _movement;
        private Direction _direction;
        private Vector _projectileMomentum;
        private double _lastFireTime;
    }
}