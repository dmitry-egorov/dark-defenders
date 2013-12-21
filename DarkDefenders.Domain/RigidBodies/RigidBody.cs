using System;
using System.Collections.Generic;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.RigidBodies
{
    public class RigidBody : RootBase<RigidBodyId, RigidBodySnapshot, IRigidBodyEventsReciever, IRigidBodyEvent>
    {
        public IEnumerable<IRigidBodyEvent> Create(WorldId worldId, Circle boundingCircle)
        {
            AssertDoesntExist();

            yield return new RigidBodyCreated(Id, worldId, boundingCircle);
        }

        public IEnumerable<IEvent> UpdateMomentum(double elapsedSeconds)
        {
            var snapshot = Snapshot;

            var world = _worldRepository.GetById(snapshot.WorldId);

            var force = GetForce(elapsedSeconds, snapshot, world);

            var newMomentum = GetNewMomentum(elapsedSeconds, world, force, snapshot);

            if (newMomentum != snapshot.Momentum)
            {
                yield return new Accelerated(Id, newMomentum);
            }
        }

        public IEnumerable<IEvent> UpdatePosition(double elapsedSeconds)
        {
            var snapshot = Snapshot;

            var world = _worldRepository.GetById(snapshot.WorldId);

            var newPosition = GetNewPosition(elapsedSeconds, snapshot, world);

            if (newPosition != snapshot.BoundingCircle.Position)
            {
                yield return new Moved(Id, newPosition);
            }
        }

        public IEnumerable<IEvent> AddMomentum(Vector additionalMomentum)
        {
            if (additionalMomentum == Vector.Zero)
            {
                yield break;
            }
            
            var snapshot = Snapshot;

            var newMomentum = snapshot.Momentum + additionalMomentum;

            yield return new Accelerated(Id, newMomentum);
        }

        public IEnumerable<IEvent> SetExternalForce(Vector force)
        {
            var snapshot = Snapshot;
            if (snapshot.ExternalForce != force)
            {
                yield return new ExternalForceChanged(Id, force);
            }
        }

        public bool IsInTheAir()
        {
            var snapshot = Snapshot;
            var world = _worldRepository.GetById(snapshot.WorldId);

            return world.IsInTheAir(snapshot.BoundingCircle);
        }

        public bool HasVerticalMomentum()
        {
            var snapshot = Snapshot;
            return Math.Abs(snapshot.Momentum.Y) > 0.01d;
        }

        public bool MomentumHasDifferentHorizontalDirectionFrom(Vector vector)
        {
            var snapshot = Snapshot;
            var px = snapshot.Momentum.X;

            return px != 0.0 && Math.Sign(px) != Math.Sign(vector.X);
        }

        internal RigidBody(RigidBodyId id, IRepository<WorldId, World> worldRepository) : base(id)
        {
            _worldRepository = worldRepository;
        }

        private static Vector GetNewMomentum(double elapsedSeconds, World world, Vector force, RigidBodySnapshot snapshot)
        {
            var momentum = snapshot.Momentum;

            var newMomentum = momentum + force * elapsedSeconds;

            newMomentum = LimitMomentum(newMomentum);

            return world.ApplyInelasticTerrainImpact(newMomentum, snapshot.BoundingCircle);
        }

        private static Vector GetNewPosition(double elapsedSeconds, RigidBodySnapshot snapshot, World world)
        {
            var momentum = snapshot.Momentum;
            var position = snapshot.BoundingCircle.Position;

            if (momentum == Vector.Zero)
            {
                return position;
            }

            var positionChange = momentum * elapsedSeconds * InverseMass;

            var newPosition = position + positionChange;

            var newCircle = snapshot.BoundingCircle.ChangePosition(newPosition);

            var adjustCirclePosition = world.AdjustCirclePosition(newCircle);

            return adjustCirclePosition;
        }

        private static Vector GetForce(double elapsedSeconds, RigidBodySnapshot snapshot, World world)
        {
            var momentum = snapshot.Momentum;

            var isInTheAir = world.IsInTheAir(snapshot.BoundingCircle);

            var externalForce = snapshot.ExternalForce;

            if (isInTheAir)
            {
                return externalForce + world.GetGravityForce(Mass);
            }

            if (externalForce != Vector.Zero)
            {
                return externalForce;
            }

            var maxForce = -momentum.X / elapsedSeconds;
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

        private static Vector GetFrictionForce(double maxForce)
        {
            var sign = Math.Sign(maxForce);
            var mfx = Math.Abs(maxForce) ;

            var frictionForce = Mass * FrictionCoefficient;

            var fx = Math.Min(mfx, frictionForce);

            return Vector.XY(sign * fx, 0);
        }

        private readonly IRepository<WorldId, World> _worldRepository;
        private const double TopHorizontalMomentum = 0.8d;
        private const double Mass = 1d;
        private const double InverseMass = 1d / Mass;
        private const double FrictionCoefficient = 2d;
    }
}