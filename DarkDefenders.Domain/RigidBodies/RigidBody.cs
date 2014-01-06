using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Math.Physics;
using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies
{
    public class RigidBody : RootBase<RigidBodyId, IRigidBodyEventsReciever, IRigidBodyEvent>, IRigidBodyEventsReciever
    {
        private const double GravityAcceleration = 200;
        private const double FrictionCoefficient = 400;

        public Vector Position { get { return _boundingBox.Center; } }

        public IEnumerable<IDomainEvent> UpdateMomentum()
        {
            if (_isTouchingTheGround && _momentumIsZero && _externalForceIsZero)
            {
                yield break;
            }

            var newMomentum = GetNewMomentum();

            yield return new Accelerated(Id, newMomentum);
        }

        public IEnumerable<IDomainEvent> UpdatePosition()
        {
            if (_momentumIsZero)
            {
                yield break;
            }

            var newPosition = GetNewPosition();

            yield return new Moved(Id, newPosition);
        }

        public IEnumerable<IDomainEvent> AddMomentum(Momentum additionalMomentum)
        {
            if (additionalMomentum.EqualsZero())
            {
                yield break;
            }
            
            var newMomentum = _momentum + additionalMomentum;

            yield return new Accelerated(Id, newMomentum);
        }

        public IEnumerable<IDomainEvent> SetExternalForce(Force force)
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
            return !_isTouchingTheGround;
        }

        public bool HasVerticalMomentum()
        {
            return Math.Abs(_momentum.Value.Y) > 0.01d;
        }

        public bool MomentumHasDifferentHorizontalDirectionFrom(Vector vector)
        {
            var momentumSign = Math.Sign(_momentum.Value.X);

            return momentumSign != 0 && momentumSign != Math.Sign(vector.X);
        }

        public bool IsTouchingAWall()
        {
            return _isTouchingAWallToTheRight
                || _isTouchingAWallToTheLeft
                || _isTouchingTheCeiling
                || _isTouchingTheGround;
        }

        public void Recieve(Moved moved)
        {
            _boundingBox = _boundingBox.ChangePosition(moved.NewPosition);

            PrepareTouching();
        }

        public void Recieve(Accelerated accelerated)
        {
            _momentum = accelerated.NewMomentum;

            PrepareMomentumIsZero();
        }

        public void Recieve(ExternalForceChanged externalForceChanged)
        {
            _externalForce = externalForceChanged.ExternalForce;

            PrepareForceIsZero();
        }

        internal RigidBody
        (
            RigidBodyId id, 
            Clock clock, 
            World world, 
            Momentum initialMomentum, 
            double mass, 
            double topHorizontalMomentum, 
            Box boundingBox
        ) 
            : base(id)
        {
            _world = world;

            _momentum = initialMomentum;
            _topHorizontalMomentum = topHorizontalMomentum;
            _boundingBox = boundingBox;
            _clock = clock;
            _externalForce = Force.Zero;
            _mass = mass;

            _gravityForce = GetGravityForce();
            PrepareTouching();
            PrepareMomentumIsZero();
            PrepareForceIsZero();
        }

        private void PrepareTouching()
        {
            _isTouchingAWallToTheRight = IsTouchingAWallToTheRight();
            _isTouchingAWallToTheLeft = IsTouchingAWallToTheLeft();
            _isTouchingTheCeiling = IsTouchingTheCeiling();
            _isTouchingTheGround = IsTouchingTheGround();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PrepareMomentumIsZero()
        {
            _momentumIsZero = _momentum.EqualsZero();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PrepareForceIsZero()
        {
            _externalForceIsZero = _externalForce.EqualsZero();
        }

        private Vector GetNewPosition()
        {
            var elapsedSeconds = _clock.GetElapsedSeconds();

            var positionChange = _momentum * elapsedSeconds * (1.0 / _mass);

            return ApplyPositionChange(positionChange);
        }

        private Momentum GetNewMomentum()
        {
            var elapsed = _clock.GetElapsedSeconds();

            var force = GetForce(elapsed);

            var newMomentum = _momentum;

            if (force.NotEqualsZero())
            {
                newMomentum += force * elapsed;

                newMomentum = LimitTopMomentum(newMomentum);
            }

            return LimitMomentumByTerrain(newMomentum);
        }

        private Force GetForce(Seconds elapsedSeconds)
        {
            var externalForce = _externalForce;

            var isTochingASurface = _isTouchingTheGround || _isTouchingTheCeiling;

            if (_externalForceIsZero && isTochingASurface)
            {
                externalForce += GetFrictionForce(elapsedSeconds);
            }

            if (!_isTouchingTheGround)
            {
                externalForce += _gravityForce;
            }

            return externalForce;
        }

        private Momentum LimitTopMomentum(Momentum momentum)
        {
            var vx = momentum.Value.X;
            var vy = momentum.Value.Y;

            var vxAbs = Math.Abs(vx);

            var topHorizontalMomentum = _topHorizontalMomentum;

            return vxAbs > topHorizontalMomentum 
                ? Vector.XY(Math.Sign(vx) * topHorizontalMomentum, vy).ToMomentum() 
                : momentum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Force GetGravityForce()
        {
            return Vector.XY(0, -_mass * GravityAcceleration).ToForce();
        }

        private Force GetFrictionForce(Seconds elapsedSeconds)
        {
            var px = _momentum.Value.X;

            var sign = - Math.Sign(px);
            var maxForce = Math.Abs(px / elapsedSeconds.Value);

            var frictionForce = _mass * FrictionCoefficient;

            var fx = Math.Min(maxForce, frictionForce);

            return Vector.XY(sign * fx, 0).ToForce();
        }

        private Momentum LimitMomentumByTerrain(Momentum momentum)
        {
            var px = momentum.Value.X;
            var py = momentum.Value.Y;

            if (px > 0)
            {
                if (_isTouchingAWallToTheRight)
                {
                    px = 0;
                }
            }
            else if (px < 0)
            {
                if (_isTouchingAWallToTheLeft)
                {
                    px = 0;
                }
            }

            if (py > 0)
            {
                if (_isTouchingTheCeiling)
                {
                    py = 0;
                }
            }
            else if (py < 0)
            {
                if (_isTouchingTheGround)
                {
                    py = 0;
                }
            }

            return Vector.XY(px, py).ToMomentum();
        }

        private bool IsTouchingAWallToTheLeft()
        {
            var center = _boundingBox.Center;
            var widthRadius = _boundingBox.WidthRadius;
            var heightRadius = _boundingBox.HeightRadius;

            var left = center.X - widthRadius;

            var x = left.PrevInteger().ToInt();

            return IsTouchingWallsAt(Axis.Vertical, center.Y, heightRadius, x);
        }

        private bool IsTouchingAWallToTheRight()
        {
            var center = _boundingBox.Center;
            var widthRadius = _boundingBox.WidthRadius;
            var heightRadius = _boundingBox.HeightRadius;

            var right = center.X + widthRadius;

            var x = right.TolerantFloor().ToInt();

            return IsTouchingWallsAt(Axis.Vertical, center.Y, heightRadius, x);
        }

        private bool IsTouchingTheGround()
        {
            var center = _boundingBox.Center;
            var heightRadius = _boundingBox.HeightRadius;
            var widthRadius = _boundingBox.WidthRadius;

            var bottom = center.Y - heightRadius;

            var y = bottom.PrevInteger().ToInt();

            return IsTouchingWallsAt(Axis.Horizontal, center.X, widthRadius, y);
        }

        private bool IsTouchingTheCeiling()
        {
            var center = _boundingBox.Center;
            var heightRadius = _boundingBox.HeightRadius;
            var widthRadius = _boundingBox.WidthRadius;

            var top = center.Y + heightRadius;

            var y = top.TolerantFloor().ToInt();

            return IsTouchingWallsAt(Axis.Horizontal, center.X, widthRadius, y);
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

                var isTouchingWalls = IsTouchingWallsAt(axis.Other(), other, otherRadius, mainToCheck);

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
        private bool IsTouchingWallsAt(Axis axis, double mainCenter, double radius, int other)
        {
            var start = (mainCenter - radius).TolerantFloor().ToInt();
            var end = (mainCenter + radius).PrevInteger().ToInt();

            return _world.AnySolidWallsAt(axis, start, end, other);
        }

        private readonly Clock _clock;
        private readonly World _world;

        private readonly double _mass;
        private readonly double _topHorizontalMomentum;
        private readonly Force _gravityForce;
        
        private Box _boundingBox;
        private Momentum _momentum;
        private Force _externalForce;

        private bool _isTouchingAWallToTheRight;
        private bool _isTouchingAWallToTheLeft;
        private bool _isTouchingTheCeiling;
        private bool _isTouchingTheGround;
        private bool _momentumIsZero;
        private bool _externalForceIsZero;
    }
}