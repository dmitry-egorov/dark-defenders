using System;
using System.Runtime.CompilerServices;
using DarkDefenders.Game.Model.EntityProperties;
using DarkDefenders.Game.Model.Events;
using DarkDefenders.Game.Model.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Physics;
using Infrastructure.Util;
using JetBrains.Annotations;

namespace DarkDefenders.Game.Model.Entities
{
    [UsedImplicitly]
    public class RigidBody : Entity<RigidBody, IRigidBodyEvents>, IRigidBodyEvents
    {
        private const double GravityAcceleration = 200.0;
        private const double FrictionCoefficient = 400.0;

        private readonly IResources<RigidBodyProperties> _resources;
        private readonly Terrain _terrain;

        private double _mass;
        private double _topHorizontalMomentum;
        
        private Force _gravityForce;
        
        private Vector _position;
        private Box _boundingBox;
        private Momentum _momentum;
        private Force _externalForce;

        private bool _isTouchingAWallToTheRight;
        private bool _isTouchingAWallToTheLeft;
        private bool _isTouchingTheCeiling;
        private bool _isTouchingTheGround;
        private bool _momentumIsZero;
        private bool _externalForceIsZero;

        public RigidBody
        (
            IResources<RigidBodyProperties> resources,
            Terrain terrain
        )
        {
            _terrain = terrain;
            _resources = resources;
        }

        public void Create(Vector initialPosition, Momentum initialMomentum, string propertiesId)
        {
            CreationEvent(x => x.Created(this, initialPosition, initialMomentum, propertiesId));
        }

        public void UpdatePhysics(TimeSpan elapsed)
        {
            var elapsedSeconds = elapsed.ToSeconds();

            Momentum momentum;
            var momentumUpdated = TryUpdateMomentum(elapsedSeconds, out momentum);

            Vector position;
            var positionUpdated = TryUpdatePosition(elapsedSeconds, momentum, out position);

            if(momentumUpdated)
            {
                Event(x => x.Accelerated(momentum));
            }
            if(positionUpdated)
            {
                Event(x => x.Moved(position));
            }
        }

        public void AddMomentum(Momentum additionalMomentum)
        {
            if (additionalMomentum.EqualsZero())
            {
                return;
            }
            
            var newMomentum = _momentum + additionalMomentum;

            Event(x => x.Accelerated(newMomentum));
        }

        public void ChangeExternalForce(Force force)
        {
            if (_externalForce.Equals(force))
            {
                return;
            }

            Event(x => x.ExternalForceChanged(force));
        }

        public void Destroy()
        {
            DestructionEvent();
        }

        public bool IsInTheAir()
        {
            return !_isTouchingTheGround;
        }

        public bool HasMomentum(Axis d)
        {
            return Math.Abs(_momentum.Value.CoordinateFor(d)) > 0.01 ;
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
            return (_position.Y - _boundingBox.HeightRadius).TolerantFloor().ToInt();
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
            var right = _position.X + _boundingBox.WidthRadius;

            return right.NextInteger().ToInt();
        }

        public int SlotToTheLeftX()
        {
            var right = _position.X - _boundingBox.WidthRadius;

            return right.TolerantFloor().ToInt() - 1;
        }

        public int SlotYUnder()
        {
            var bottom = _position.Y - _boundingBox.HeightRadius;

            return bottom.PrevInteger().ToInt();
        }

        public Vector GetPositionOffsetBy(Axis axis, double offset)
        {
            if (axis == Axis.Horizontal)
            {
                return Vector.XY(_position.X + offset, _position.Y);
            }
            if (axis == Axis.Vertical)
            {
                return Vector.XY(_position.X, _position.Y + offset);
            }

            throw new ArgumentException("Invalid value", "axis");
        }

        void IRigidBodyEvents.Created(RigidBody rigidBody, Vector initialPosition, Momentum initialMomentum, string propertiesId)
        {
            var properties = _resources[propertiesId];

            var radius = properties.BoundingBoxRadius;

            _momentum = initialMomentum;
            _topHorizontalMomentum = properties.TopHorizontalMomentum;
            _position = initialPosition;
            _boundingBox = new Box(radius, radius);
            _mass = properties.Mass;

            _externalForce = Force.Zero;
            _gravityForce = GetGravityForce();

            PrepareTouching();
            PrepareMomentumIsZero();
            PrepareForceIsZero();
        }

        void IRigidBodyEvents.Accelerated(Momentum newMomentum)
        {
            _momentum = newMomentum;

            PrepareMomentumIsZero();
        }

        void IRigidBodyEvents.Moved(Vector newPosition)
        {
            _position = newPosition;

            PrepareTouching();
        }

        void IRigidBodyEvents.ExternalForceChanged(Force externalForce)
        {
            _externalForce = externalForce;

            PrepareForceIsZero();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryUpdateMomentum(Seconds elapsed, out Momentum newMomentum)
        {
            if (_isTouchingTheGround && _momentumIsZero && _externalForceIsZero)
            {
                newMomentum = _momentum;
                return false;
            }

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
        private bool TryUpdatePosition(Seconds elapsed, Momentum momentum, out Vector newPosition)
        {
            if (momentum.EqualsZero())
            {
                newPosition = _position;
                return false;
            }

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

            return IsTouchingWallsAt(Axis.Vertical, _position.Y, _boundingBox.HeightRadius, x);
        }

        private bool CalculateIsTouchingAWallToTheRight()
        {
            var x = RightBoundSlotX();

            return IsTouchingWallsAt(Axis.Vertical, _position.Y, _boundingBox.HeightRadius, x);
        }

        private bool CalculateIsTouchingTheGround()
        {
            var y = BottomBoundSlotY();

            return IsTouchingWallsAt(Axis.Horizontal, _position.X, _boundingBox.WidthRadius, y);
        }

        private bool CalculateIsTouchingTheCeiling()
        {
            var y = TopBoundSlotY();

            return IsTouchingWallsAt(Axis.Horizontal, _position.X, _boundingBox.WidthRadius, y);
        }

        private Vector ApplyPositionChange(Vector positionDelta)
        {
            var center = _position;

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
            var center = _position;

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
            var bottom = _position.Y - _boundingBox.HeightRadius;

            return bottom.PrevInteger().ToInt();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int TopBoundSlotY()
        {
            var top = _position.Y + _boundingBox.HeightRadius;

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
            var left = _position.X - _boundingBox.WidthRadius;

            return left.PrevInteger().ToInt();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int RightBoundSlotX()
        {
            var right = _position.X + _boundingBox.WidthRadius;

            return right.TolerantFloor().ToInt();
        }
    }
}