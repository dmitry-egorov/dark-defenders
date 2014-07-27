using System;
using System.Drawing;
using DarkDefenders.Game.Model.Entities;
using Infrastructure.Math;

namespace DarkDefenders.Game.Model.States.Heroes
{
    internal class Moving : IHeroState
    {
        private readonly Creature _creature;
        private readonly HeroStateFactory _stateFactory;
        private readonly RigidBody _rigidBody;
        private readonly Terrain _terrain;

        public Moving(HeroStateFactory stateFactory, RigidBody rigidBody, Creature creature, Terrain terrain)
        {
            _creature = creature;
            _terrain = terrain;
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

            var objectInSpace = _rigidBody.GetBoundingBox();

            var direction = _creature.GetDirection().ToDirection();

            var x = objectInSpace.NextSlot(direction);
            var y = objectInSpace.BoundSlot(Direction.Bottom);

            return new Point(x, y);
        }

        private bool IsMovingIntoAWall()
        {
            switch (_creature.GetDirection())
            {
                case HorizontalDirection.Left:
                    return _rigidBody.IsTouchingAWallToTheLeft();
                case HorizontalDirection.Right:
                    return _rigidBody.IsTouchingAWallToTheRight();
                default:
                    throw new InvalidOperationException("Invalid direction.");
            }
        }

        private bool CanJumpOver()
        {
            //TODO: refactor, use line transformations
            //TODO: depends on parameters
            var maxJumpHeight = 2;

            var direction = _creature.GetDirection().ToDirection();

            var objectInSpace = _rigidBody.GetBoundingBox();

            var x = objectInSpace.BoundSlot(direction);
            var y = objectInSpace.BoundSlot(Direction.Bottom);
            
            var line = DiscreteAxisAlignedLine.From(Axis.Y, y + 1, y + maxJumpHeight, x);

            return _terrain.AnyOpenWallsAt(line);
        }
    }
}