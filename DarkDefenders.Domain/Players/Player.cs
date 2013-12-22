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

        public IEnumerable<IEvent> Create(WorldId worldId)
        {
            AssertDoesntExist();
            foreach (var e in CreatePlayer(worldId)) { yield return e; }
        }

        public IEnumerable<IEvent> MoveLeft()
        {
            AssertExists();
            foreach (var e in SetMovementForce(MovementForceDirection.Left)) { yield return e; }
        }

        public IEnumerable<IEvent> MoveRight()
        {
            AssertExists();
            foreach (var e in SetMovementForce(MovementForceDirection.Right)) { yield return e; }
        }

        public IEnumerable<IEvent> Stop()
        {
            AssertExists();
            foreach (var e in SetMovementForce(MovementForceDirection.Stop)) { yield return e; }
        }

        public IEnumerable<IEvent> Jump()
        {
            AssertExists();

            if (CantJump())
            {
                yield break;
            }

            foreach (var e in AddJumpMomentum()) { yield return e; }
        }

        public IEnumerable<IEvent> Fire()
        {
            AssertExists();

            if (FireDelayInEffect())
            {
                yield break;
            }

            foreach (var e in CreateProjectile()) { yield return e; }
        }

        public IEnumerable<IEvent> ApplyMovementForce()
        {
            AssertExists();
            foreach (var e in SetMovementForceToRigidBody()) { yield return e; }
        }

        public void Apply(PlayerCreated playerCreated)
        {
            _world = _worldRepository.GetById(playerCreated.WorldId);
            _rigidBody = _rigidBodyRepository.GetById(playerCreated.RigidBodyId);
            _movementForceDirection = MovementForceDirection.Stop;
            _direction = Direction.Right;
        }

        public void Apply(MovementForceChanged movementForceChanged)
        {
            var movementForce = movementForceChanged.MovementForceDirection;
            _movementForceDirection = movementForce;
            _direction = GetDirectionFrom(movementForce);
        }

        public void Apply(ProjectileCreated projectileCreated)
        {
            var projectile = new Projectile();

            _projectiles.Add(projectileCreated.ProjectileId, projectile);

            _lastFireTime = projectileCreated.Time;
        }

        internal Player(PlayerId id, IRepository<WorldId, World> worldRepository, IRepository<RigidBodyId, RigidBody> rigidBodyRepository) 
            : base(id)
        {
            _worldRepository = worldRepository;
            _rigidBodyRepository = rigidBodyRepository;
        }

        private IEnumerable<IEvent> CreatePlayer(WorldId worldId)
        {
            var world = _worldRepository.GetById(worldId);

            RigidBodyId rigidBodyId;
            foreach (var e in CreateOwnRigidBody(world, out rigidBodyId)) { yield return e; }

            yield return new PlayerCreated(Id, worldId, rigidBodyId);
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
            return _world.WorldElapsedSeconds - _lastFireTime < FireDelay;
        }

        private Direction GetDirectionFrom(MovementForceDirection movementForceDirection)
        {
            switch (movementForceDirection)
            {
                case MovementForceDirection.Left:
                    return Direction.Left;
                case MovementForceDirection.Right:
                    return Direction.Right;
                default:
                    return _direction;
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
                    return Vector.Left * MovementForce;
                case MovementForceDirection.Right:
                    return Vector.Right * MovementForce;
                default:
                    throw new ArgumentOutOfRangeException("desiredMovementForceDirection");
            }
        }

        private IEnumerable<IEvent> CreateProjectile()
        {
            RigidBodyId rigidBodyId;
            foreach (var e in CreateProjectileRigidBody(out rigidBodyId)) { yield return e; }

            var projectileId = new ProjectileId();

            yield return new ProjectileCreated(Id, projectileId, rigidBodyId, _world.WorldElapsedSeconds);
        }

        private IEnumerable<IRigidBodyEvent> CreateOwnRigidBody(World world, out RigidBodyId rigidBodyId)
        {
            var spawnPosition = world.GetSpawnPosition();

            return CreateRigidBody(world.Id, spawnPosition, Vector.Zero, BoundingCircleRadius, Mass, out rigidBodyId);
        }

        private IEnumerable<IRigidBodyEvent> CreateProjectileRigidBody(out RigidBodyId rigidBodyId)
        {
            var momentumSign = _direction == Direction.Right ? 1.0 : -1.0;

            var offset = Vector.XY(momentumSign * _rigidBody.Radius, 0.0);
            var position = _rigidBody.Position + offset;
            var momentum = Vector.XY(momentumSign * Projectile.Momentum, 0.0);

            return CreateRigidBody(_world.Id, position, momentum, Projectile.BoundingCircleRadius, Projectile.Mass, out rigidBodyId);
        }

        private IEnumerable<IRigidBodyEvent> CreateRigidBody(WorldId worldId, Vector position, Vector momentum, double radius, double mass, out RigidBodyId rigidBodyId)
        {
            var boundingCircle = new Circle(position, radius);

            rigidBodyId = new RigidBodyId();

            var rigidBody = _rigidBodyRepository.GetById(rigidBodyId);

            return rigidBody.Create(worldId, boundingCircle, momentum, mass);
        }

        private static readonly Vector _jumpMomentum = Vector.XY(0, 1.5d);
        private const double MovementForce = 4d;
        private const double FireDelay = 0.25;

        private readonly IRepository<WorldId, World> _worldRepository;
        private readonly IRepository<RigidBodyId, RigidBody> _rigidBodyRepository;

        private readonly Dictionary<ProjectileId, Projectile> _projectiles = new Dictionary<ProjectileId, Projectile>();

        private World _world;
        private RigidBody _rigidBody;
        private MovementForceDirection _movementForceDirection;
        private Direction _direction;
        private double _lastFireTime;
    }
}
