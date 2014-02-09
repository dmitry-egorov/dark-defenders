using System;
using System.Collections.Generic;
using System.Drawing;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Creatures.Events;
using DarkDefenders.Domain.Entities.Other;
using DarkDefenders.Domain.Entities.Projectiles;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Entities.Terrains;
using DarkDefenders.Domain.Factories;
using DarkDefenders.Domain.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace DarkDefenders.Domain.Entities.Creatures
{
    public class Creature: Entity<Creature>
    {
        private static readonly TimeSpan _fireDelay = TimeSpan.FromSeconds(0.25);
        private const double ProjectileMomentum = 150.0 * Projectile.Mass;

        private const Direction InitialDirection = Direction.Right;
        private readonly Momentum _jumpMomentum;
        private readonly Force _leftMovementForce;
        private readonly Force _rightMovementForce;

        private static readonly Momentum _leftProjectileMomentum = Vector.XY(-ProjectileMomentum, 0).ToMomentum();
        private static readonly Momentum _rightProjectileMomentum = Vector.XY(ProjectileMomentum, 0).ToMomentum();

        private readonly IStorage<Creature> _storage;
        private readonly ProjectileFactory _projectileFactory;
        private readonly Clock _clock;
        private readonly Terrain _terrain;
        private readonly RigidBody _rigidBody;
        private readonly Cooldown _fireCooldown;

        private Movement _movement;
        private Direction _direction;
        private Momentum _projectileMomentum;

        internal Creature(IStorage<Creature> storage, ProjectileFactory projectileFactory, Clock clock, Terrain terrain, RigidBody rigidBody, CreatureProperties properties) 
        {
            _terrain = terrain;
            _rigidBody = rigidBody;
            _storage = storage;
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

        public IEnumerable<IEvent> ChangeMovement(Movement movement)
        {
            if (_movement == movement)
            {
                yield break;
            }

            yield return new MovementChanged(this, movement);

            var movementForce = GetMovementForceFor(movement);

            var events = _rigidBody.ChangeExternalForce(movementForce);

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IEvent> Jump()
        {
            if (CantJump())
            {
                yield break;
            }

            var events = AddJumpMomentum();

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IEvent> Fire()
        {
            if (_fireCooldown.IsInEffect())
            {
                yield break;
            }
            
            var events = CreateProjectile();

            foreach (var e in events) { yield return e; }

            var currentTime = _clock.GetCurrentTime();

            yield return new Fired(this, currentTime);
        }

        public IEnumerable<IEvent> InvertMovement()
        {
            return ChangeMovement(_movement.Other());
        }

        public IEnumerable<IEvent> Kill()
        {
            yield return new CreatureDestroyed(this, _storage);

            var events = _rigidBody.Destroy();

            foreach (var e in events) { yield return e; }
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

            var terrain = _terrain;
            for (var x = xStart; (xEnd - x) * sign >= 0; x += sign)
            {
                if(!terrain.AnyOpenWallsAt(Axis.Vertical, yStart, yEnd, x))
                {
                    return false;
                }
            }

            return true;
        }

        internal void MovementChanged(Movement movement)
        {
            _movement = movement;
            _direction = GetDirection();
            _projectileMomentum = GetProjectileMomentum();
        }

        internal void Fired(TimeSpan activationTime)
        {
            _fireCooldown.SetLastActivationTime(activationTime);
        }

        private bool CantJump()
        {
            return _rigidBody.IsInTheAir() || _rigidBody.HasVerticalMomentum();
        }

        private IEnumerable<IEvent> AddJumpMomentum()
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

        private Force GetMovementForceFor(Movement desiredMovement)
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

        private IEnumerable<IEvent> CreateProjectile()
        {
            var projectilePosition = GetProjectilePosition();

            return _projectileFactory.Create(projectilePosition, _projectileMomentum);
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
    }
}