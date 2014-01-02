using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.RigidBodies
{
    public class RigidBody : RootBase<RigidBodyId, IRigidBodyEventsReciever, IRigidBodyEvent>, IRigidBodyEventsReciever
    {
        private const double GravityAcceleration = 200;
        private const double FrictionCoefficient = 200.0;

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

        public bool IsInTheAir()
        {
            return _world.IsInTheAir(_boundingBox);
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

        public bool IsAdjacentToAWall()
        {
            return _world.IsAdjacentToAWall(_boundingBox);
        }

        public IEnumerable<IDomainEvent> Destroy()
        {
            yield return new RigidBodyDestroyed(Id);
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

            return _world.ApplyPositionChange(_boundingBox, positionChange);
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

            return _world.LimitMomentum(newMomentum, _boundingBox);
        }

        private Vector GetForce(double elapsedSeconds)
        {
            var isInTheAir = _world.IsInTheAir(_boundingBox);

            var externalForce = _externalForce;

            if (isInTheAir)
            {
                return externalForce + GetGravityForce();
            }

            if (externalForce.EqualsZero())
            {
                return GetFrictionForce(elapsedSeconds);
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
            var maxForce = -_momentum.X / elapsedSeconds;

            var sign = Math.Sign(maxForce);
            var mfx = Math.Abs(maxForce) ;

            var frictionForce = _mass * FrictionCoefficient;

            var fx = Math.Min(mfx, frictionForce);

            return Vector.XY(sign * fx, 0);
        }

        private readonly World _world;
        
        private readonly double _mass;
        private readonly double _topHorizontalMomentum;
        
        private Box _boundingBox;
        private Vector _momentum;
        private Vector _externalForce;
    }
}