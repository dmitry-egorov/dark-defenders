using System;
using System.Drawing;
using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Game.Model.Other;

namespace DarkDefenders.Game.Model.States.Heroes
{
    internal class Moving : IHeroState
    {
        private readonly Creature _creature;
        private readonly HeroStateFactory _stateFactory;
        private readonly RigidBody _rigidBody;

        public Moving(HeroStateFactory stateFactory, RigidBody rigidBody, Creature creature)
        {
            _creature = creature;
            _stateFactory = stateFactory;
            _rigidBody = rigidBody;
        }

        public void Update()
        {
            if (_rigidBody.IsInTheAir())
            {
                var fallenFrom = GetFallingFrom();

                _stateFactory.Falling(fallenFrom);

                return;
            }

            if (!IsMovingIntoAWall())
            {
                return;
            }

            if (CanJumpOver())
            {
                _creature.TryJump();

                _stateFactory.Jumping();
            }
            else
            {
                _creature.InvertMovement();
            }
        }

        private Point GetFallingFrom()
        {
            if (!_rigidBody.IsInTheAir())
            {
                throw new InvalidOperationException("Is not in the air");
            }

            var x = _rigidBody.NextSlotX(_creature.GetDirection().Other());
            var y = _rigidBody.SlotYUnder();

            return new Point(x, y);
        }

        private bool IsMovingIntoAWall()
        {
            switch (_creature.GetDirection())
            {
                case Direction.Left:
                    return _rigidBody.IsTouchingAWallToTheLeft();
                case Direction.Right:
                    return _rigidBody.IsTouchingAWallToTheRight();
                default:
                    throw new InvalidOperationException("Invalid movement state.");
            }
        }

        public bool CanJumpOver()
        {
            //TODO: depends on parameters
            var maxJumpHeight = 2;

            return _rigidBody.AreOpeningsNextTo(_creature.GetDirection(), 1, maxJumpHeight);
        }
    }
}