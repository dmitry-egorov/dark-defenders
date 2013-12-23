using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Players.Entities.Projectiles;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Players
{
    public class Player : RootBase<PlayerId, IPlayerEventsReciever, IPlayerEvent>, IPlayerEventsReciever
    {
        public const double BoundingCircleRadius = 1.0 / 40.0;
        public const double Mass = 1.0;

        public IEnumerable<IEvent> MoveLeft()
        {
            foreach (var e in SetMovementForce(MovementForceDirection.Left)) { yield return e; }
        }

        public IEnumerable<IEvent> MoveRight()
        {
            foreach (var e in SetMovementForce(MovementForceDirection.Right)) { yield return e; }
        }

        public IEnumerable<IEvent> Stop()
        {
            foreach (var e in SetMovementForce(MovementForceDirection.Stop)) { yield return e; }
        }

        public IEnumerable<IEvent> Jump()
        {
            if (CantJump())
            {
                yield break;
            }

            foreach (var e in AddJumpMomentum()) { yield return e; }
        }

        public IEnumerable<IEvent> Fire()
        {
            if (FireDelayInEffect())
            {
                yield break;
            }

            foreach (var e in CreateProjectile()) { yield return e; }
        }

        public IEnumerable<IEvent> ApplyMovementForce()
        {
            foreach (var e in SetMovementForceToRigidBody()) { yield return e; }
        }

        public IEnumerable<IEvent> UpdateProjectiles()
        {
            foreach (var projectile in _projectiles.Values)
            {
                foreach (var e in projectile.CheckForHit()) { yield return e; }
            }
        }

        public void Recieve(MovementForceChanged movementForceChanged)
        {
            var movementForce = movementForceChanged.MovementForceDirection;
            _movementForceDirection = movementForce;
            PrepareDirection();
            PrepareProjectileMomentum();
        }

        public void Recieve(ProjectileCreated projectileCreated)
        {
            var rigidBody = _rigidBodyRepository.GetById(projectileCreated.RigidBodyId);
            
            var projectile = new Projectile(rigidBody, Id, projectileCreated.ProjectileId);

            _projectiles.Add(projectileCreated.ProjectileId, projectile);

            _lastFireTime = projectileCreated.Time;
        }

        public void Recieve(ProjectileHitSomething projectileHitSomething)
        {
            _projectiles.Remove(projectileHitSomething.ProjectileId);
        }

        internal Player(PlayerId id, RigidBodyFactory rigidBodyFactory, IRepository<RigidBodyId, RigidBody> rigidBodyRepository, World world, RigidBody rigidBody) 
            : base(id)
        {
            _rigidBodyFactory = rigidBodyFactory;
            _rigidBodyRepository = rigidBodyRepository;

            _world = world;
            _rigidBody = rigidBody;

            _movementForceDirection = MovementForceDirection.Stop;
            _direction = InitialDirection;
            PrepareProjectileMomentum();
        }

        private bool CantJump()
        {
            return _rigidBody.IsInTheAir() || _rigidBody.HasVerticalMomentum();
        }

        private IEnumerable<IEvent> AddJumpMomentum()
        {
            return _rigidBody.AddMomentum(_jumpMomentum);
        }

        private IEnumerable<IEvent> SetMovementForceToRigidBody()
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

        private IEnumerable<IEvent> SetMovementForce(MovementForceDirection movementForceDirection)
        {
            if (_movementForceDirection != movementForceDirection)
            {
                yield return new MovementForceChanged(Id, movementForceDirection);
            }
        }

        private Vector GetMovementForce()
        {
            var force = GetMovementForce(_movementForceDirection);

            if (_rigidBody.IsInTheAir())
            {
                force *= 0.5d;
            }
            else if (_rigidBody.MomentumHasDifferentHorizontalDirectionFrom(force))
            {
                force *= 2d;
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

        private IEnumerable<IEvent> CreateProjectile()
        {
            RigidBodyId rigidBodyId;
            foreach (var e in CreateProjectileRigidBody(out rigidBodyId)) { yield return e; }

            var projectileId = new ProjectileId();

            yield return new ProjectileCreated(Id, projectileId, rigidBodyId, _world.TimeSeconds);
        }

        private IEnumerable<IEvent> CreateProjectileRigidBody(out RigidBodyId rigidBodyId)
        {
            var position = GetProjectilePosition();

            rigidBodyId = new RigidBodyId();
            return _rigidBodyFactory.CreateRigidBody(rigidBodyId, _world.Id, position, Projectile.BoundingCircleRadius, _projectileMomentum, Projectile.Mass);
        }

        private void PrepareProjectileMomentum()
        {
            _projectileMomentum = 
                _direction == Direction.Right 
                ? Projectile.RightMomentum 
                : Projectile.LeftMomentum;
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

        private static readonly Vector _jumpMomentum = Vector.XY(0, 1.5d);
        private static readonly Vector _leftMovementForce = Vector.Left * MovementForce;
        private static readonly Vector _rightMovementForce = Vector.Right * MovementForce;
        private const double MovementForce = 4d;
        private const double FireDelay = 0.25;
        private const Direction InitialDirection = Direction.Right;

        private readonly RigidBodyFactory _rigidBodyFactory;
        private readonly IRepository<RigidBodyId, RigidBody> _rigidBodyRepository;

        private readonly World _world;
        private readonly RigidBody _rigidBody;

        private readonly Dictionary<ProjectileId, Projectile> _projectiles = new Dictionary<ProjectileId, Projectile>();

        private MovementForceDirection _movementForceDirection;
        private Direction _direction;
        private Vector _projectileMomentum;
        private double _lastFireTime;
    }
}
