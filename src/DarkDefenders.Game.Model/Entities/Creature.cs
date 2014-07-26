using System;
using DarkDefenders.Game.Model.EntityProperties;
using DarkDefenders.Game.Model.Events;
using DarkDefenders.Game.Model.Other;
using DarkDefenders.Kernel.Model;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using JetBrains.Annotations;

namespace DarkDefenders.Game.Model.Entities
{
    [UsedImplicitly]
    public class Creature : Entity<Creature, ICreatureEvents>, ICreatureEvents
    {
        private const Direction InitialDirection = Direction.Right;

        private readonly IResources<CreatureProperties> _resources;

        private RigidBody _rigidBody;
        
        private double _movementForceAmplitude;
        
        private Movement _movement;
        private Direction _direction;
        private Momentum _jumpMomentum;

        public Creature(IResources<CreatureProperties> resources)
        {
            _resources = resources;

            _movement = Movement.Stop;
            _direction = InitialDirection;
        }

        public void Create(RigidBody rigidBody, string propertiesId)
        {
            CreationEvent(x => x.Created(this, rigidBody, propertiesId));
        }

        public void ChangeMovementTo(Movement movement)
        {
            if (_movement == movement)
            {
                return;
            }

            var direction = GetDirectionFrom(movement);

            Event(x => x.MovementChanged(movement, direction));

            var movementForce = GetMovementForceFor(movement);

            _rigidBody.ChangeExternalForce(movementForce);
        }

        public void TryJump()
        {
            if (CantJump())
            {
                return;
            }

            AddJumpMomentum();
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

        public Direction GetDirection()
        {
            return _direction;
        }

        void ICreatureEvents.Created(Creature creature, RigidBody rigidBody, string propertiesId)
        {
            _rigidBody = rigidBody;

            var properties = _resources[propertiesId];

            _movementForceAmplitude = properties.MovementForce;

            _jumpMomentum = Vector.XY(0, properties.JumpMomentum).ToMomentum();
        }

        void ICreatureEvents.MovementChanged(Movement movement, Direction direction)
        {
            _movement = movement;
            _direction = direction;
        }

        private bool CantJump()
        {
            return _rigidBody.IsInTheAir() || _rigidBody.HasMomentum(Axis.Vertical);
        }

        private void AddJumpMomentum()
        {
            _rigidBody.AddMomentum(_jumpMomentum);
        }

        private Direction GetDirectionFrom(Movement movement)
        {
            switch (movement)
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

            if (MomentumHasDifferentHorizontalDirectionFrom(force.Value))
            {
                force *= 4.0;
            }

            if (_rigidBody.IsInTheAir())
            {
                force *= 0.5;
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
                    return Force.Left * _movementForceAmplitude;
                case Movement.Right:
                    return Force.Right * _movementForceAmplitude;
                default:
                    throw new ArgumentOutOfRangeException("desiredMovement");
            }
        }

        private bool MomentumHasDifferentHorizontalDirectionFrom(Vector vector)
        {
            var momentumSign = _rigidBody.GetMomentum().Value.Sign(Axis.Horizontal);

            return momentumSign != 0 && momentumSign != vector.Sign(Axis.Horizontal);
        }
    }
}