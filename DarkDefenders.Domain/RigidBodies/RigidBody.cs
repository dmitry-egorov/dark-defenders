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
        private readonly IRepository<WorldId, World> _worldRepository;
        private const double BoundingCircleRaius = 1d / 40d;
        private const double TopHorizontalMomentum = 0.8d;
        private const double Mass = 1d;
        private const double InverseMass = 1d / Mass;

        internal RigidBody(RigidBodyId id, IRepository<WorldId, World> worldRepository) : base(id)
        {
            _worldRepository = worldRepository;
        }

        public IEnumerable<IRigidBodyEvent> Create(WorldId worldId, Vector initialPosition)
        {
            AssertDoesntExist();

            yield return new RigidBodyCreated(Id, worldId, initialPosition);
        }

        public IEnumerable<IEvent> UpdateKineticState(TimeSpan elapsed)
        {
            var snapshot = Snapshot;

            var world = _worldRepository.GetById(snapshot.WorldId);
            
            foreach (var @event in ChangePosition(elapsed, snapshot, world)) yield return @event;
            foreach (var @event in ChangeMomentum(elapsed, snapshot, world)) yield return @event;
        }

        private IEnumerable<IEvent> ChangePosition(TimeSpan elapsed, RigidBodySnapshot snapshot, World world)
        {
            var newPosition = GetNewPosition(elapsed, snapshot, world);

            if (newPosition != snapshot.Position)
            {
                yield return new Moved(Id, newPosition);
            }
        }

        private static Vector GetNewPosition(TimeSpan elapsed, RigidBodySnapshot snapshot, World world)
        {
            var momentum = snapshot.Momentum;
            var position = snapshot.Position;

            if (momentum == Vector.Zero)
            {
                return position;
            }

            var positionChange = momentum * elapsed.TotalSeconds * InverseMass;

            var newPosition = position + positionChange;

            return world.AdjustPosition(newPosition, BoundingCircleRaius);
        }

        private IEnumerable<IEvent> ChangeMomentum(TimeSpan elapsed, RigidBodySnapshot snapshot, World world)
        {
            var force = GetForce(elapsed, snapshot, world);

            var newMomentum = GetNewMomentum(elapsed, world, force, snapshot);

            if (newMomentum != snapshot.Momentum)
            {
                yield return new Accelerated(Id, newMomentum);
            }
        }

        private static Vector GetNewMomentum(TimeSpan elapsed, World world, Vector force, RigidBodySnapshot snapshot)
        {
            var momentum = snapshot.Momentum;
            var position = snapshot.Position;

            var newMomentum = momentum + force * elapsed.TotalSeconds;

            newMomentum = LimitMomentum(newMomentum);

            return world.ApplyInelasticTerrainImpact(position, BoundingCircleRaius, newMomentum);
        }

        private static Vector GetForce(TimeSpan elapsed, RigidBodySnapshot snapshot, World world)
        {
            var momentum = snapshot.Momentum;
            var position = snapshot.Position;

            var isInTheAir = world.IsInTheAir(position, BoundingCircleRaius);

            var externalForce = snapshot.ExternalForce;

            if (isInTheAir)
            {
                return externalForce + world.GetGravityForce(Mass);
            }
            
            if (externalForce == Vector.Zero)
            {
                return externalForce + world.GetFrictionForce(momentum, Mass, elapsed);
            }

            return externalForce;
        }

        private static Vector LimitMomentum(Vector momentum)
        {
            var vx = momentum.X;
            var vy = momentum.Y;

            return Math.Abs(vx) > TopHorizontalMomentum 
                ? Vector.XY(Math.Sign(vx) * TopHorizontalMomentum, vy) 
                : momentum;
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

            return world.IsInTheAir(snapshot.Position, BoundingCircleRaius);
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
    }
}