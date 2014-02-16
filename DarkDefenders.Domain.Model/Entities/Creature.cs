using System;
using System.Drawing;
using DarkDefenders.Domain.Model.EntityProperties;
using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities
{
    [UsedImplicitly]
    public class Creature : Entity<Creature, ICreatureEvents>, ICreatureEvents
    {
        private const Direction InitialDirection = Direction.Right;

        private readonly IResources<CreatureProperties> _resources;
        private readonly Terrain _terrain;
        private readonly RigidBody _rigidBody;
        private readonly Weapon _weapon;
        
        private Movement _movement;
        private Direction _direction;
        private Momentum _jumpMomentum;

        private Force _leftMovementForce;
        private Force _rightMovementForce;

        public Creature
        (
            IResources<CreatureProperties> resources, 
            RigidBody rigidBody, 
            Weapon weapon, 
            Terrain terrain
        )
        {
            _rigidBody = rigidBody;

            _terrain = terrain;
            _resources = resources;
            _weapon = weapon;

            _movement = Movement.Stop;
            _direction = InitialDirection;
        }

        public void Create(Vector initialPosition, string propertiesId)
        {
            _rigidBody.Create(initialPosition, Momentum.Zero, propertiesId);
            _weapon.Create(_rigidBody);

            CreationEvent(x => x.Created(this, _rigidBody, propertiesId));
        }

        public void ChangeMovementTo(Movement movement)
        {
            if (_movement == movement)
            {
                return;
            }

            Event(x => x.MovementChanged(movement));

            var movementForce = GetMovementForceFor(movement);

            _rigidBody.ChangeExternalForce(movementForce);
        }

        public void Jump()
        {
            if (CantJump())
            {
                return;
            }

            AddJumpMomentum();
        }

        public void Fire()
        {
            _weapon.Fire(_direction);
        }

        public void InvertMovement()
        {
            ChangeMovementTo(_movement.Other());
        }

        public void Kill()
        {
            DestructionEvent();

            _rigidBody.Destroy();
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

        void ICreatureEvents.Created(Creature creature, RigidBody rigidBody, string propertiesId)
        {
            var properties = _resources[propertiesId];

            _rightMovementForce = Force.Right * properties.MovementForce;
            _leftMovementForce = Force.Left * properties.MovementForce;
            _jumpMomentum = Vector.XY(0, properties.JumpMomentum).ToMomentum();
        }

        void ICreatureEvents.MovementChanged(Movement movement)
        {
            _movement = movement;
            _direction = GetDirection();
        }

        private bool CantJump()
        {
            return _rigidBody.IsInTheAir() || _rigidBody.HasVerticalMomentum();
        }

        private void AddJumpMomentum()
        {
            _rigidBody.AddMomentum(_jumpMomentum);
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