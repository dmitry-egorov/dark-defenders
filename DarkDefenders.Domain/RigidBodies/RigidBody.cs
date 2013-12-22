using System;
using System.Collections.Generic;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.RigidBodies
{
    public class RigidBody : RootBase<RigidBodyId, IRigidBodyEventsReciever, IRigidBodyEvent>, IRigidBodyEventsReciever
    {
        public Vector Position
        {
            get
            {
                AssertExists();
                return _boundingCircle.Position;
            }
        }

        public double Radius
        {
            get
            {
                AssertExists();
                return _boundingCircle.Radius;
            }
        }

        public IEnumerable<IRigidBodyEvent> Create(WorldId worldId, Circle boundingCircle, Vector initialMomentum, double mass)
        {
            AssertDoesntExist();

            yield return new RigidBodyCreated(Id, worldId, boundingCircle, initialMomentum, mass);
        }

        public IEnumerable<IEvent> UpdateMomentum(double elapsedSeconds)
        {
            AssertExists();

            var newMomentum = GetNewMomentum(elapsedSeconds);

            if (newMomentum == _momentum)
            {
                yield break;
            }

            yield return new Accelerated(Id, newMomentum);
        }

        public IEnumerable<IEvent> UpdatePosition(double elapsedSeconds)
        {
            AssertExists();

            if (_momentum == Vector.Zero)
            {
                yield break;
            }

            var newPosition = GetNewPosition(elapsedSeconds);

            yield return new Moved(Id, newPosition);
        }

        public IEnumerable<IEvent> AddMomentum(Vector additionalMomentum)
        {
            AssertExists();
            
            if (additionalMomentum == Vector.Zero)
            {
                yield break;
            }
            
            var newMomentum = _momentum + additionalMomentum;

            yield return new Accelerated(Id, newMomentum);
        }

        public IEnumerable<IEvent> SetExternalForce(Vector force)
        {
            AssertExists();
            
            if (_externalForce == force)
            {
                yield break;
            }

            yield return new ExternalForceChanged(Id, force);
        }

        public bool IsInTheAir()
        {
            AssertExists();
            return _world.IsInTheAir(_boundingCircle);
        }

        public bool HasVerticalMomentum()
        {
            AssertExists();
            return Math.Abs(_momentum.Y) > 0.01d;
        }

        public bool MomentumHasDifferentHorizontalDirectionFrom(Vector vector)
        {
            AssertExists();

            var momentumSign = Math.Sign(_momentum.X);

            return momentumSign != 0 && momentumSign != Math.Sign(vector.X);
        }

        public void Apply(RigidBodyCreated rigidBodyCreated)
        {
            _world = _worldRepository.GetById(rigidBodyCreated.WorldId);

            _momentum = rigidBodyCreated.InitialMomentum;
            _mass = rigidBodyCreated.Mass;
            _boundingCircle = rigidBodyCreated.BoundingCircle;
            _externalForce = Vector.Zero;
        }

        public void Apply(Moved moved)
        {
            _boundingCircle = _boundingCircle.ChangePosition(moved.NewPosition);
        }

        public void Apply(Accelerated accelerated)
        {
            _momentum = accelerated.NewMomentum;
        }

        public void Apply(ExternalForceChanged externalForceChanged)
        {
            _externalForce = externalForceChanged.ExternalForce;
        }

        internal RigidBody(RigidBodyId id, IRepository<WorldId, World> worldRepository) : base(id)
        {
            _worldRepository = worldRepository;
        }

        private Vector GetNewMomentum(double elapsedSeconds)
        {
            var force = GetForce(elapsedSeconds);

            var newMomentum = _momentum + force * elapsedSeconds;

            newMomentum = LimitMomentum(newMomentum);

            return _world.ApplyInelasticTerrainImpact(newMomentum, _boundingCircle);
        }

        private Vector GetNewPosition(double elapsedSeconds)
        {
            var positionChange = _momentum * elapsedSeconds * (1.0 / _mass);

            var newPosition = _boundingCircle.Position + positionChange;

            var newCircle = _boundingCircle.ChangePosition(newPosition);

            return _world.AdjustCirclePosition(newCircle);
        }

        private Vector GetForce(double elapsedSeconds)
        {
            var isInTheAir = _world.IsInTheAir(_boundingCircle);

            var externalForce = _externalForce;

            if (isInTheAir)
            {
                return externalForce + _world.GetGravityForce(_mass);
            }

            if (externalForce != Vector.Zero)
            {
                return externalForce;
            }

            var maxForce = -_momentum.X / elapsedSeconds;

            return externalForce + GetFrictionForce(maxForce);
        }

        private static Vector LimitMomentum(Vector momentum)
        {
            var vx = momentum.X;
            var vy = momentum.Y;

            return Math.Abs(vx) > TopHorizontalMomentum 
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

        private readonly IRepository<WorldId, World> _worldRepository;

        private World _world;
        private Circle _boundingCircle;
        private Vector _momentum;
        private Vector _externalForce;
        private double _mass;
    }
}