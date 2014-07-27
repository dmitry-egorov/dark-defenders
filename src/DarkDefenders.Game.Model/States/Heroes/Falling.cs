using System;
using System.Drawing;
using DarkDefenders.Game.Model.Entities;
using Infrastructure.Math;

namespace DarkDefenders.Game.Model.States.Heroes
{
    internal class Falling : IHeroState
    {
        private readonly Creature _creature;
        private readonly HeroStateFactory _factory;
        private readonly RigidBody _rigidBody;
        private readonly Point _fallenFrom;
        private readonly Terrain _terrain;

        public Falling(HeroStateFactory factory, RigidBody rigidBody, Creature creature, Point fallenFrom, Terrain terrain)
        {
            _creature = creature;
            _fallenFrom = fallenFrom;
            _terrain = terrain;
            _factory = factory;
            _rigidBody = rigidBody;
        }

        public void Update()
        {
            if (_rigidBody.IsInTheAir())
            {
                return;
            }

            if (CanMoveBackwardsAfterFall(_fallenFrom))
            {
                _factory.Deciding();
            }
            else
            {
                _factory.Moving();
            }
        }

        private bool CanMoveBackwardsAfterFall(Point fallenFrom)
        {
            //TODO: refactor, use line transformation
            var boundingBox = _rigidBody.GetBoundingBox();

            var yStart = boundingBox.BoundSlot(Direction.Bottom);
            var yEnd = Math.Min(fallenFrom.Y, yStart + 2);

            var direction = _creature.GetDirection().ToDirection().Other();

            var xStart = boundingBox.NextSlot(direction);
            var xEnd = fallenFrom.X;
            var sign = direction.GetIncrement();

            for (var x = xStart; (xEnd - x) * sign >= 0; x += sign)
            {
                var line = DiscreteAxisAlignedLine.From(Axis.Y, yStart, yEnd, x);

                if (!_terrain.AnyOpenWallsAt(line))
                {
                    return false;
                }
            }

            return true;
        }
    }
}