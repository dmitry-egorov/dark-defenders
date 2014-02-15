using System;
using System.Collections.Generic;
using System.Drawing;
using DarkDefenders.Domain.Model.Entities.Creatures.Events;
using DarkDefenders.Domain.Model.Entities.RigidBodies;
using DarkDefenders.Domain.Model.Entities.Terrains;
using DarkDefenders.Domain.Model.Entities.Weapons;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using Infrastructure.Util;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities.Creatures
{
    [UsedImplicitly]
    public class Creature : Entity<Creature>
    {
        private const Direction InitialDirection = Direction.Right;

        private readonly IStorage<Creature> _storage;
        private readonly Terrain _terrain;
        private readonly RigidBody _rigidBody;
        private readonly Weapon _weapon;
        
        private Movement _movement;
        private Direction _direction;
        private Momentum _jumpMomentum;

        private Force _leftMovementForce;
        private Force _rightMovementForce;

        public Creature(IStorage<Creature> storage, RigidBody rigidBody, Weapon weapon, Terrain terrain) 
        {
            _storage = storage;
            _rigidBody = rigidBody;

            _terrain = terrain;
            _weapon = weapon;

            _movement = Movement.Stop;
            _direction = InitialDirection;
        }

        public IEnumerable<IEvent> Create(Vector initialPosition, CreatureProperties properties)
        {
            var revents = _rigidBody.Create(initialPosition, Momentum.Zero, properties.RigidBodyProperties);
            var wevents = _weapon.Create(_rigidBody);

            var events = Concat.All(revents, wevents);
            foreach (var e in events) { yield return e; }

            yield return new CreatureCreated(this, _storage, _rigidBody, properties);
        }

        public IEnumerable<IEvent> ChangeMovementTo(Movement movement)
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
            var events = _weapon.Fire(_direction);

            foreach (var e in events) { yield return e; }
        }

        public IEnumerable<IEvent> InvertMovement()
        {
            return ChangeMovementTo(_movement.Other());
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

        internal void Created(RigidBody rigidBody, CreatureProperties properties)
        {
            _rightMovementForce = Force.Right * properties.MovementForce;
            _leftMovementForce = Force.Left * properties.MovementForce;
            _jumpMomentum = Vector.XY(0, properties.JumpMomentum).ToMomentum();
        }

        internal void MovementChanged(Movement movement)
        {
            _movement = movement;
            _direction = GetDirection();
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
    }
}