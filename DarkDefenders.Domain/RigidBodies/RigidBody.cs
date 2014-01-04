using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies
{
    public class RigidBody : RootBase<RigidBodyId, IRigidBodyEventsReciever, IRigidBodyEvent>, IRigidBodyEventsReciever
    {
        private const double GravityAcceleration = 200;
        private const double FrictionCoefficient = 300;

        public Vector Position { get { return _boundingBox.Center; } }

        public IEnumerable<IDomainEvent> UpdateMomentum()
        {
            var elapsedSeconds = _world.ElapsedSeconds;

            var newMomentum = GetNewMomentum(elapsedSeconds);

            if (newMomentum.Equals(_momentum))
            {
                yield break;
            }

            yield return new Accelerated(Id, newMomentum);
        }

        public IEnumerable<IDomainEvent> UpdatePosition()
        {
            var elapsedSeconds = _world.ElapsedSeconds;

            if (_momentum.EqualsZero())
            {
                yield break;
            }

            var newPosition = GetNewPosition(elapsedSeconds);

            yield return new Moved(Id, newPosition);
        }

        public IEnumerable<IDomainEvent> AddMomentum(Vector additionalMomentum)
        {
            if (additionalMomentum.EqualsZero())
            {
                yield break;
            }
            
            var newMomentum = _momentum + additionalMomentum;

            yield return new Accelerated(Id, newMomentum);
        }

        public IEnumerable<IDomainEvent> SetExternalForce(Vector force)
        {
            if (_externalForce.Equals(force))
            {
                yield break;
            }

            yield return new ExternalForceChanged(Id, force);
        }

        public IEnumerable<IDomainEvent> Destroy()
        {
            yield return new RigidBodyDestroyed(Id);
        }

        public bool IsInTheAir()
        {
            return !IsTouchingTheGround();
        }

        public bool HasVerticalMomentum()
        {
            return Math.Abs(_momentum.Y) > 0.01d;
        }

        public bool MomentumHasDifferentHorizontalDirectionFrom(Vector vector)
        {
            var momentumSign = Math.Sign(_momentum.X);

            return momentumSign != 0 && momentumSign != Math.Sign(vector.X);
        }

        public bool IsTouchingAWall()
        {
            return IsTouchingAWallToTheRight()
                || IsTouchingAWallToTheLeft()
                || IsTouchingTheCeiling()
                || IsTouchingTheGround();
        }

        public void Recieve(Moved moved)
        {
            _boundingBox = _boundingBox.ChangePosition(moved.NewPosition);
        }

        public void Recieve(Accelerated accelerated)
        {
            _momentum = accelerated.NewMomentum;
        }

        public void Recieve(ExternalForceChanged externalForceChanged)
        {
            _externalForce = externalForceChanged.ExternalForce;
        }

        internal RigidBody
            (
                RigidBodyId id, 
                World world, 
                Vector initialMomentum, 
                double mass, 
                double topHorizontalMomentum, 
                Box boundingBox
            ) : base(id)
        {
            _world = world;

            _momentum = initialMomentum;
            _mass = mass;
            _topHorizontalMomentum = topHorizontalMomentum;
            _boundingBox = boundingBox;
            _externalForce = Vector.Zero;
        }

        private Vector GetNewPosition(double elapsedSeconds)
        {
            var positionChange = _momentum * (elapsedSeconds / _mass);

            return ApplyPositionChange(positionChange);
        }

        private Vector GetNewMomentum(double elapsedSeconds)
        {
            var force = GetForce(elapsedSeconds);

            var newMomentum = _momentum;

            if (force.NotEqualsZero())
            {
                newMomentum += force * elapsedSeconds;

                newMomentum = LimitTopMomentum(newMomentum);
            }

            return LimitMomentumByTerrain(newMomentum);
        }

        private Vector GetForce(double elapsedSeconds)
        {
            var isTouchingTheGround = IsTouchingTheGround();

            var externalForce = _externalForce;

            if (externalForce.EqualsZero() && isTouchingTheGround || IsTouchingTheCeiling())
            {
                externalForce += GetFrictionForce(elapsedSeconds);
            }

            if (!isTouchingTheGround)
            {
                externalForce += GetGravityForce();
            }

            return externalForce;
        }

        private Vector LimitTopMomentum(Vector momentum)
        {
            var vx = momentum.X;
            var vy = momentum.Y;

            var vxAbs = Math.Abs(vx);

            var topHorizontalMomentum = _topHorizontalMomentum;

            return vxAbs > topHorizontalMomentum 
                ? Vector.XY(Math.Sign(vx) * topHorizontalMomentum, vy) 
                : momentum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector GetGravityForce()
        {
            return Vector.XY(0, -_mass * GravityAcceleration);
        }

        private Vector GetFrictionForce(double elapsedSeconds)
        {
            var sign = - Math.Sign(_momentum.X);
            var maxForce = Math.Abs(_momentum.X / elapsedSeconds);

            var frictionForce = _mass * FrictionCoefficient;

            var fx = Math.Min(maxForce, frictionForce);

            return Vector.XY(sign * fx, 0);
        }

        private Vector LimitMomentumByTerrain(Vector momentum)
        {
            var px = momentum.X;
            var py = momentum.Y;

            if (px > 0)
            {
                if (IsTouchingAWallToTheRight())
                {
                    px = 0;
                }
            }
            else if (px < 0)
            {
                if (IsTouchingAWallToTheLeft())
                {
                    px = 0;
                }
            }

            if (py > 0)
            {
                if (IsTouchingTheCeiling())
                {
                    py = 0;
                }
            }
            else if (py < 0)
            {
                if (IsTouchingTheGround())
                {
                    py = 0;
                }
            }

            return Vector.XY(px, py);
        }

        private bool IsTouchingAWallToTheLeft()
        {
            var center = _boundingBox.Center;
            var widthRadius = _boundingBox.WidthRadius;
            var heightRadius = _boundingBox.HeightRadius;

            var left = center.X - widthRadius;

            var x = left.PrevInteger().ToInt();

            return IsTouchingWallsOn(Axis.Vertical, center.Y, heightRadius, x);
        }

        private bool IsTouchingAWallToTheRight()
        {
            var center = _boundingBox.Center;
            var widthRadius = _boundingBox.WidthRadius;
            var heightRadius = _boundingBox.HeightRadius;

            var right = center.X + widthRadius;

            var x = right.TolerantFloor().ToInt();

            return IsTouchingWallsOn(Axis.Vertical, center.Y, heightRadius, x);
        }

        private bool IsTouchingTheGround()
        {
            var center = _boundingBox.Center;
            var heightRadius = _boundingBox.HeightRadius;
            var widthRadius = _boundingBox.WidthRadius;

            var bottom = center.Y - heightRadius;

            var y = bottom.PrevInteger().ToInt();

            return IsTouchingWallsOn(Axis.Horizontal, center.X, widthRadius, y);
        }

        private bool IsTouchingTheCeiling()
        {
            var center = _boundingBox.Center;
            var heightRadius = _boundingBox.HeightRadius;
            var widthRadius = _boundingBox.WidthRadius;

            var top = center.Y + heightRadius;

            var y = top.TolerantFloor().ToInt();

            return IsTouchingWallsOn(Axis.Horizontal, center.X, widthRadius, y);
        }

        private Vector ApplyPositionChange(Vector positionDelta)
        {
            var center = _boundingBox.Center;

            Vector horizontalAdjustment;
            var horizontalPositionAdjusted = ApplyAxisPositionChange(Axis.Horizontal, positionDelta, out horizontalAdjustment);

            Vector verticalAdjustment;
            var verticalPositionAdjusted = ApplyAxisPositionChange(Axis.Vertical, positionDelta, out verticalAdjustment);

            if (verticalPositionAdjusted && horizontalPositionAdjusted)
            {
                var horizontalDelta = (horizontalAdjustment - center).LengthSquared();
                var verticalDelta = (verticalAdjustment - center).LengthSquared();

                return horizontalDelta < verticalDelta ? horizontalAdjustment : verticalAdjustment;
            }

            if (horizontalPositionAdjusted)
            {
                return horizontalAdjustment;
            }

            if (verticalPositionAdjusted)
            {
                return verticalAdjustment;
            }

            return center + positionDelta;
        }

        private bool ApplyAxisPositionChange(Axis axis, Vector positionDelta, out Vector adjustedPosition)
        {
            var center = _boundingBox.Center;

            var mainCenter = center.CoordinateFor(axis);
            var otherCenter = center.CoordinateFor(axis.Other());

            var mainRadius = _boundingBox.RadiusFor(axis);
            var otherRadius = _boundingBox.RadiusFor(axis.Other());

            var dMain = positionDelta.CoordinateFor(axis);
            var dOther = positionDelta.CoordinateFor(axis.Other());

            if (dMain == 0.0)
            {
                adjustedPosition = Vector.Zero;
                return false;
            }

            var sign = Math.Sign(dMain);
            var slope = dOther / dMain;

            var boundOffset = sign * mainRadius;
            var startBoundOther = otherCenter;
            var startBoundMain = mainCenter + boundOffset;
            var endBoundMain = startBoundMain + dMain;

            var start = ((sign == 1) ? startBoundMain.TolerantCeiling() : startBoundMain.TolerantFloor()).ToInt();
            var end = ((sign == 1) ? endBoundMain.PrevInteger() : endBoundMain.NextInteger()).ToInt();

            for (var main = start; (end - main) * sign >= 0; main += sign)
            {
                var mainToCheck = (sign == 1) ? main : main - 1;
                var other = slope * (main - startBoundMain) + startBoundOther;

                var isTouchingWalls = IsTouchingWallsOn(axis.Other(), other, otherRadius, mainToCheck);

                if (!isTouchingWalls)
                {
                    continue;
                }

                adjustedPosition = Vector.ByAxis(axis, main - boundOffset, other);
                return true;
            }

            adjustedPosition = Vector.Zero;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsTouchingWallsOn(Axis axis, double mainCenter, double radius, int other)
        {
            var start = (mainCenter - radius).TolerantFloor().ToInt();
            var end = (mainCenter + radius).PrevInteger().ToInt();

            for (var main = start; main <= end; main++)
            {
                var isSolid = _world.IsTerrainSolidAt(axis, main, other);

                if (isSolid)
                {
                    return true;
                }
            }

            return false;
        }

        private readonly World _world;
        
        private readonly double _mass;
        private readonly double _topHorizontalMomentum;
        
        private Box _boundingBox;
        private Vector _momentum;
        private Vector _externalForce;
    }
}