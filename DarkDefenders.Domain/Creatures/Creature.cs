using System;
using System.Collections.Generic;
using System.Drawing;
using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Creatures.Events;
using DarkDefenders.Domain.Projectiles;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Terrains;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Creatures
{
    public class Creature : RootBase<CreatureId, ICreatureEventsReciever, ICreatureEvent>, ICreatureEventsReciever
    {
        private static readonly TimeSpan _fireDelay = TimeSpan.FromSeconds(0.25);
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

            var currentTime = _clock.GetCurrentTime();

            yield return new CreatureFired(Id, currentTime);
        }

        public IEnumerable<IDomainEvent> InvertMovement()
        {
            return SetMovement(_movement.Other());
        }

        public IEnumerable<IDomainEvent> Kill()
        {
            yield return new CreatureDestroyed(Id);

            var events = _rigidBody.Destroy();

            foreach (var e in events) { yield return e; }
        }

        public void Recieve(MovementChanged movementChanged)
        {
            _movement = movementChanged.Movement;
            _direction = GetDirection();
            _projectileMomentum = GetProjectileMomentum();
        }

        public void Recieve(CreatureFired creatureFired)
        {
            _fireCooldown.SetLastActivationTime(creatureFired.Time);
        }

        public bool IsInTheAir()
        {
            return _rigidBody.IsInTheAir();
        }

        public Point GetFallingFrom()
        {
            if (!_rigidBody.IsInTheAir())
            {
                throw new InvalidOperationException("Is not in the air");
            }

            var x = _rigidBody.NextSlotX(_movement.ToDirection().Other());
            var y = _rigidBody.SlotYUnder();

            return new Point(x, y);
        }

        public bool IsMovingIntoAWall()
        {
            switch (_movement)
            {
                case Movement.Stop:
                    return false;
                case Movement.Left:
                    return _rigidBody.IsTouchingAWallToTheLeft();
                case Movement.Right:
                    return _rigidBody.IsTouchingAWallToTheRight();
                default:
                    throw new InvalidOperationException("Invalid movement state.");
            }
        }

        public bool CanJumpOver()
        {
            //TODO: depends on parameters
            var maxJumpHeight = 2;

            return _rigidBody.AreOpeningsNextTo(_movement.ToDirection(), 1, maxJumpHeight);
        }

        public bool CanMoveBackwardsAfterFall(Point fallenFrom)
        {
            var yStart = _rigidBody.Level();
            var yEnd = Math.Min(fallenFrom.Y, yStart + 2);

            var direction = _movement.ToDirection().Other();

            var xStart = _rigidBody.NextSlotX(direction);
            var xEnd = fallenFrom.X;
            var sign = direction.GetXIncrement();

            for (var x = xStart; (xEnd - x)*sign >= 0 ; x += sign)
            {
                if(!_terrain.AnyOpenWallsAt(Axis.Vertical, yStart, yEnd, x))
                {
                    return false;
                }
            }

            return true;
        }

        internal Creature(CreatureId id, ProjectileFactory projectileFactory, Clock clock, Terrain terrain, RigidBody rigidBody, CreatureProperties properties) 
            : base(id)
        {
            
            _terrain = terrain;
            _rigidBody = rigidBody;
            _clock = clock;
            _projectileFactory = projectileFactory;

            _movement = Movement.Stop;
            _direction = InitialDirection;
            _projectileMomentum = GetProjectileMomentum();

            _fireCooldown = new Cooldown(clock, _fireDelay);

            _rightMovementForce = Force.Right * properties.MovementForce;
            _leftMovementForce = Force.Left * properties.MovementForce;
            _jumpMomentum = Vector.XY(0, properties.JumpMomentum).ToMomentum();
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

        private Force GetMovementForceDirection(Movement desiredMovement)
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

            return _projectileFactory.Create(projectileId, _clock.Id, _terrain.Id, projectilePosition, _projectileMomentum);
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

        private readonly Momentum _jumpMomentum;
        private readonly Force _leftMovementForce;
        private readonly Force _rightMovementForce;

        private static readonly Momentum _leftProjectileMomentum = Vector.XY(-ProjectileMomentum, 0).ToMomentum();
        private static readonly Momentum _rightProjectileMomentum = Vector.XY(ProjectileMomentum, 0).ToMomentum();

        private readonly ProjectileFactory _projectileFactory;
        private readonly Clock _clock;
        private readonly Terrain _terrain;
        private readonly RigidBody _rigidBody;
        private readonly Cooldown _fireCooldown;

        private Movement _movement;
        private Direction _direction;
        private Momentum _projectileMomentum;
    }
}