using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DarkDefenders.Domain.Model.Entities.Clocks;
using DarkDefenders.Domain.Model.Entities.RigidBodies.Events;
using DarkDefenders.Domain.Model.Entities.Terrains;
using DarkDefenders.Domain.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using Infrastructure.Util;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Model.Entities.RigidBodies
{
    [UsedImplicitly]
    public class RigidBody : Entity<RigidBody>
    {
        private const double GravityAcceleration = 200.0;
        private const double FrictionCoefficient = 400.0;

        private readonly IStorage<RigidBody> _storage;
        private readonly Clock _clock;
        private readonly Terrain _terrain;

        private double _mass;
        private double _topHorizontalMomentum;
        private Force _gravityForce;
        
        private Box _boundingBox;
        private Momentum _momentum;
        private Force _externalForce;

        private bool _isTouchingAWallToTheRight;
        private bool _isTouchingAWallToTheLeft;
        private bool _isTouchingTheCeiling;
        private bool _isTouchingTheGround;
        private bool _momentumIsZero;
        private bool _externalForceIsZero;

        public Vector Position { get { return _boundingBox.Center; } }

        public RigidBody
        (
            IStorage<RigidBody> storage, 
            Clock clock, 
            Terrain terrain
        )
        {
            _storage = storage;
            _clock = clock;
            _terrain = terrain;
        }

        public IEnumerable<IEvent> Create(Vector initialPosition, Momentum initialMomentum, RigidBodyProperties properties)
        {
            yield return new RigidBodyCreated(this, _storage, initialPosition, initialMomentum, properties);
        }

        public IEnumerable<IEvent> UpdatePhysics()
        {
            Momentum momentum;
            var momentumUpdated = TryUpdateMomentum(out momentum);

            Vector position;
            var positionUpdated = TryUpdatePosition(momentum, out position);

            if (momentumUpdated && positionUpdated)
            {
                yield return new AcceleratedAndMoved(this, momentum, position);
            }
            else if(momentumUpdated)
            {
                yield return new Accelerated(this, momentum);
            }
            else if(positionUpdated)
            {
                yield return new Moved(this, position);
            }
        }

        public IEnumerable<IEvent> AddMomentum(Momentum additionalMomentum)
        {
            if (additionalMomentum.EqualsZero())
            {
                yield break;
            }
            
            var newMomentum = _momentum + additionalMomentum;

            yield return new Accelerated(this, newMomentum);
        }

        public IEnumerable<IEvent> ChangeExternalForce(Force force)
        {
            if (_externalForce.Equals(force))
            {
                yield break;
            }

            yield return new ExternalForceChanged(this, force);
        }

        public IEnumerable<IEvent> Destroy()
        {
            yield return new RigidBodyDestroyed(this, _storage);
        }

        public void SetExternalForce(Force externalForce)
        {
            _externalForce = externalForce;

            PrepareForceIsZero();
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

        public bool IsTouchingAWallToTheLeft()
        {
            return _isTouchingAWallToTheLeft;
        }

        public bool IsTouchingAWallToTheRight()
        {
            return _isTouchingAWallToTheRight;
        }

        public bool IsTouchingAnyWalls()
        {
            return _isTouchingAWallToTheRight
                || _isTouchingAWallToTheLeft
                || _isTouchingTheCeiling
                || _isTouchingTheGround;
        }

        public bool AreOpeningsNextTo(Direction direction, int heightStart, int heightEnd)
        {
            var x = BoundSlotX(direction);
            var y = Level();

            return _terrain.AnyOpenWallsAt(Axis.Vertical, y + heightStart, y + heightEnd, x);
        }

        public int Level()
        {
            return (_boundingBox.Center.Y - _boundingBox.HeightRadius).TolerantFloor().ToInt();
        }

        public int NextSlotX(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return SlotToTheLeftX();
                case Direction.Right:
                    return SlotToTheRightX();
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public int SlotToTheRightX()
        {
            var right = _boundingBox.Center.X + _boundingBox.WidthRadius;

            return right.NextInteger().ToInt();
        }

        public int SlotToTheLeftX()
        {
            var right = _boundingBox.Center.X - _boundingBox.WidthRadius;

            return right.TolerantFloor().ToInt() - 1;
        }

        public int SlotYUnder()
        {
            var bottom = _boundingBox.Center.Y - _boundingBox.HeightRadius;

            return bottom.PrevInteger().ToInt();
        }

        internal void Created(Vector initialPosition, Momentum initialMomentum, RigidBodyProperties properties)
        {
            var radius = properties.BoundingBoxRadius;

            _momentum = initialMomentum;
            _topHorizontalMomentum = properties.TopHorizontalMomentum;
            _boundingBox = new Box(initialPosition, radius, radius);
            _mass = properties.Mass;

            _externalForce = Force.Zero;
            _gravityForce = GetGravityForce();

            PrepareTouching();
            PrepareMomentumIsZero();
            PrepareForceIsZero();
        }

        internal void Accelerated(Momentum newMomentum)
        {
            _momentum = newMomentum;

            PrepareMomentumIsZero();
        }

        internal void Moved(Vector newPosition)
        {
            _boundingBox = _boundingBox.ChangePosition(newPosition);

            PrepareTouching();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryUpdateMomentum(out Momentum newMomentum)
        {
            if (_isTouchingTheGround && _momentumIsZero && _externalForceIsZero)
            {
                newMomentum = _momentum;
                return false;
            }

            var elapsed = _clock.GetElapsedSeconds();

            var force = GetForce(elapsed);

            if (force.NotEqualsZero())
            {
                newMomentum = _momentum + force * elapsed;

                newMomentum = LimitTopMomentum(newMomentum);
            }
            else
            {
                newMomentum = _momentum;
            }

            newMomentum = LimitMomentumByTerrain(newMomentum);

            return !newMomentum.Equals(_momentum);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryUpdatePosition(Momentum momentum, out Vector newPosition)
        {
            if (momentum.EqualsZero())
            {
                newPosition = _boundingBox.Center;
                return false;
            }

            var elapsed = _clock.GetElapsedSeconds();

            var positionChange = momentum * elapsed * (1.0 / _mass);

            newPosition = ApplyPositionChange(positionChange);
            return true;
        }

        private void PrepareTouching()
        {
            _isTouchingAWallToTheRight = CalculateIsTouchingAWallToTheRight();
            _isTouchingAWallToTheLeft = CalculateIsTouchingAWallToTheLeft();
            _isTouchingTheCeiling = CalculateIsTouchingTheCeiling();
            _isTouchingTheGround = CalculateIsTouchingTheGround();
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

        private bool CalculateIsTouchingAWallToTheLeft()
        {
            var x = LeftBoundSlotX();

            return IsTouchingWallsAt(Axis.Vertical, _boundingBox.Center.Y, _boundingBox.HeightRadius, x);
        }

        private bool CalculateIsTouchingAWallToTheRight()
        {
            var x = RightBoundSlotX();

            return IsTouchingWallsAt(Axis.Vertical, _boundingBox.Center.Y, _boundingBox.HeightRadius, x);
        }

        private bool CalculateIsTouchingTheGround()
        {
            var y = BottomBoundSlotY();

            return IsTouchingWallsAt(Axis.Horizontal, _boundingBox.Center.X, _boundingBox.WidthRadius, y);
        }

        private bool CalculateIsTouchingTheCeiling()
        {
            var y = TopBoundSlotY();

            return IsTouchingWallsAt(Axis.Horizontal, _boundingBox.Center.X, _boundingBox.WidthRadius, y);
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

            return _terrain.AnySolidWallsAt(axis, start, end, other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int BottomBoundSlotY()
        {
            var bottom = _boundingBox.Center.Y - _boundingBox.HeightRadius;

            return bottom.PrevInteger().ToInt();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int TopBoundSlotY()
        {
            var top = _boundingBox.Center.Y + _boundingBox.HeightRadius;

            return top.TolerantFloor().ToInt();
        }

        private int BoundSlotX(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return LeftBoundSlotX();
                case Direction.Right:
                    return RightBoundSlotX();
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int LeftBoundSlotX()
        {
            var left = _boundingBox.Center.X - _boundingBox.WidthRadius;

            return left.PrevInteger().ToInt();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int RightBoundSlotX()
        {
            var right = _boundingBox.Center.X + _boundingBox.WidthRadius;

            return right.TolerantFloor().ToInt();
        }
    }
}