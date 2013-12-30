using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.RigidBodies
{
    public class RigidBody : RootBase<RigidBodyId, IRigidBodyEventsReciever, IRigidBodyEvent>, IRigidBodyEventsReciever
    {
        public Vector Position { get { return _boundingCircle.Position; } }

        public double Radius { get { return _boundingCircle.Radius; } }

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
            return _world.IsInTheAir(_boundingCircle);
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
            return _world.IsAdjacentToAWall(_boundingCircle);
        }

        public IEnumerable<IDomainEvent> Destroy()
        {
            yield return new RigidBodyDestroyed(Id);
        }

        public void Recieve(Moved moved)
        {
            _boundingCircle = _boundingCircle.ChangePosition(moved.NewPosition);
        }

        public void Recieve(Accelerated accelerated)
        {
            _momentum = accelerated.NewMomentum;
        }

        public void Recieve(ExternalForceChanged externalForceChanged)
        {
            _externalForce = externalForceChanged.ExternalForce;
        }

        internal RigidBody(RigidBodyId id, World world, Vector initialMomentum, double mass, Circle boundingCircle) : base(id)
        {
            _world = world;

            _momentum = initialMomentum;
            _mass = mass;
            _boundingCircle = boundingCircle;
            _externalForce = Vector.Zero;
        }

        private Vector GetNewPosition(double elapsedSeconds)
        {
            var positionChange = _momentum * (elapsedSeconds / _mass);

            var newPosition = _boundingCircle.Position + positionChange;

            var newCircle = _boundingCircle.ChangePosition(newPosition);

            return _world.AdjustCirclePosition(newCircle);
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

            return _world.LimitMomentum(newMomentum, _boundingCircle);
        }

        private Vector GetForce(double elapsedSeconds)
        {
            var isInTheAir = _world.IsInTheAir(_boundingCircle);

            var externalForce = _externalForce;

            if (isInTheAir)
            {
                return externalForce + _world.GetGravityForce(_mass);
            }

            if (externalForce.EqualsZero())
            {
                var maxForce = -_momentum.X/elapsedSeconds;

                return externalForce + GetFrictionForce(maxForce);
            }

            return externalForce;
        }

        private static Vector LimitTopMomentum(Vector momentum)
        {
            var vx = momentum.X;
            var vy = momentum.Y;

            var vxAbs = Math.Abs(vx);

            return vxAbs > TopHorizontalMomentum 
                ? Vector.XY(Math.Sign(vx) * TopHorizontalMomentum, vy) 
                : momentum;
        }

        private Vector GetFrictionForce(double maxForce)
        {
            var sign = Math.Sign(maxForce);
            var mfx = Math.Abs(maxForce) ;

            var frictionForce = _mass * FrictionCoefficient;

            var fx = Math.Min(mfx, frictionForce);

            return Vector.XY(sign * fx, 0);
        }

        private const double TopHorizontalMomentum = 0.8d;
        private const double FrictionCoefficient = 2d;

        private readonly World _world;
        
        private readonly double _mass;

        private Circle _boundingCircle;
        private Vector _momentum;

        private Vector _externalForce;
    }
}