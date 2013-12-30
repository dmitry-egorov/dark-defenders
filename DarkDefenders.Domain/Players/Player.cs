using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.Projectiles;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Players
{
    public class Player : RootBase<PlayerId, IPlayerEventsReciever, IPlayerEvent>, IPlayerEventsReciever
    {
        public const double BoundingCircleRadius = 0.5;
        public const double Mass = 1.0;
        public const double TopHorizontalMomentum = 60.0;

        private const double MovementForce = 200.0;
        private const double FireDelay = 0.25;
        private const double JumpMomentum = 60;
        private const double ProjectileMomentum = 150.0 * Projectile.Mass;

        private const Direction InitialDirection = Direction.Right;

        
        public IEnumerable<IDomainEvent> MoveLeft()
        {
            var events = SetMovementForce(MovementForceDirection.Left);

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IDomainEvent> MoveRight()
        {
            var events = SetMovementForce(MovementForceDirection.Right);

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IDomainEvent> Stop()
        {
            var events = SetMovementForce(MovementForceDirection.Stop);

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IDomainEvent> Jump()
        {
            if (CantJump())
            {
                yield break;
            }

            foreach (var e in AddJumpMomentum()) { yield return e; }
        }

        public IEnumerable<IDomainEvent> Fire()
        {
            if (FireDelayInEffect())
            {
                yield break;
            }
            
            var events = CreateProjectile();

            foreach (var e in events) { yield return e; }

            yield return new PlayerFired(Id, _world.TimeSeconds);
        }

        public IEnumerable<IDomainEvent> ApplyMovementForce()
        {
            var events = SetMovementForceToRigidBody();

            foreach (var e in events) { yield return e; }
        }

        public void Recieve(MovementForceDirectionChanged movementForceDirectionChanged)
        {
            _movementForceDirection = movementForceDirectionChanged.MovementForceDirection;
            PrepareDirection();
            PrepareProjectileMomentum();
        }

        public void Recieve(PlayerFired playerFired)
        {
            _lastFireTime = playerFired.Time;
        }

        internal Player(PlayerId id, ProjectileFactory projectileFactory, World world, RigidBody rigidBody) 
            : base(id)
        {
            _world = world;
            _rigidBody = rigidBody;
            _projectileFactory = projectileFactory;

            _movementForceDirection = MovementForceDirection.Stop;
            _direction = InitialDirection;
            PrepareProjectileMomentum();
        }

        private bool CantJump()
        {
            return _rigidBody.IsInTheAir() || _rigidBody.HasVerticalMomentum();
        }

        private IEnumerable<IDomainEvent> AddJumpMomentum()
        {
            return _rigidBody.AddMomentum(_jumpMomentum);
        }

        private IEnumerable<IDomainEvent> SetMovementForceToRigidBody()
        {
            var force = GetMovementForce();

            return _rigidBody.SetExternalForce(force);
        }

        private bool FireDelayInEffect()
        {
            return _world.TimeSeconds - _lastFireTime < FireDelay;
        }

        private void PrepareDirection()
        {
            switch (_movementForceDirection)
            {
                case MovementForceDirection.Left:
                    _direction = Direction.Left;
                    break;
                case MovementForceDirection.Right:
                    _direction = Direction.Right;
                    break;
            }
        }

        private IEnumerable<IDomainEvent> SetMovementForce(MovementForceDirection movementForceDirection)
        {
            if (_movementForceDirection != movementForceDirection)
            {
                yield return new MovementForceDirectionChanged(Id, movementForceDirection);
            }
        }

        private Vector GetMovementForce()
        {
            var force = GetMovementForce(_movementForceDirection);

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

        private static Vector GetMovementForce(MovementForceDirection desiredMovementForceDirection)
        {
            switch (desiredMovementForceDirection)
            {
                case MovementForceDirection.Stop:
                    return Vector.Zero;
                case MovementForceDirection.Left:
                    return _leftMovementForce;
                case MovementForceDirection.Right:
                    return _rightMovementForce;
                default:
                    throw new ArgumentOutOfRangeException("desiredMovementForceDirection");
            }
        }

        private IEnumerable<IDomainEvent> CreateProjectile()
        {
            var projectilePosition = GetProjectilePosition();

            var projectileId = new ProjectileId();

            return _projectileFactory.Create(projectileId, _world.Id, projectilePosition, _projectileMomentum);
        }

        private void PrepareProjectileMomentum()
        {
            _projectileMomentum = 
                _direction == Direction.Right 
                ? _rightProjectileMomentum 
                : _leftProjectileMomentum;
        }

        private Vector GetProjectilePosition()
        {
            var position = _rigidBody.Position;
            var x = position.X;
            var y = position.Y;

            if (_direction == Direction.Right)
            {
                x += _rigidBody.Radius;
            }
            else
            {
                x -= _rigidBody.Radius;
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

        private MovementForceDirection _movementForceDirection;
        private Direction _direction;
        private Vector _projectileMomentum;
        private double _lastFireTime;
    }
}