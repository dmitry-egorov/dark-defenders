using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Players
{
    public class Player : RootBase<PlayerId, PlayerSnapshot, IPlayerEventsReciever, IPlayerEvent>
    {
        public const double BoundingCircleRaius = 1d / 40d;

        public IEnumerable<IEvent> Create(WorldId worldId)
        {
            AssertDoesntExist();

            var world = _worldRepository.GetById(worldId);

            var spawnPosition = world.GetSpawnPosition();
            var boundingCircle = new Circle(spawnPosition, BoundingCircleRaius);

            var rigidBody = _rigidBodyRepository.GetNew();

            foreach (var e in rigidBody.Create(worldId, boundingCircle)) { yield return e; }

            yield return new PlayerCreated(Id, worldId, rigidBody.Id);
        }

        public IEnumerable<IEvent> MoveLeft()
        {
            return SetMovementForce(Other.MovementForce.Left);
        }

        public IEnumerable<IEvent> MoveRight()
        {
            return SetMovementForce(Other.MovementForce.Right);
        }

        public IEnumerable<IEvent> TryJump()
        {
            var snapshot = Snapshot;
            var rigidBody = _rigidBodyRepository.GetById(snapshot.RigidBodyId);

            if (rigidBody.IsInTheAir() || rigidBody.HasVerticalMomentum())
            {
                yield break;
            }

            foreach (var e in rigidBody.AddMomentum(_jumpMomentum)) { yield return e; }
        }

        public IEnumerable<IEvent> Stop()
        {
            return SetMovementForce(Other.MovementForce.Stop);
        }

        public IEnumerable<IEvent> Fire()
        {
//            var cantFire = Snapshot.CantFire;
//
//            if (cantFire)
//            {
//                yield break;
//            }

            //yield return 
            yield break;
        }

        public IEnumerable<IEvent> ApplyMovementForce()
        {
            var snapshot = Snapshot;
            var rigidBody = _rigidBodyRepository.GetById(snapshot.RigidBodyId);

            var force = GetMovementForce(snapshot, rigidBody);

            foreach (var e in rigidBody.SetExternalForce(force)) { yield return e; }
        }

        internal Player(PlayerId id, IRepository<WorldId, World> worldRepository, IRepository<RigidBodyId, RigidBody> rigidBodyRepository) : base(id)
        {
            _worldRepository = worldRepository;
            _rigidBodyRepository = rigidBodyRepository;
        }

        private IEnumerable<IEvent> SetMovementForce(MovementForce movementForce)
        {
            if (Snapshot.MovementForce != movementForce)
            {
                yield return new MovementForceChanged(Id, movementForce);
            }
        }

        private static Vector GetMovementForce(PlayerSnapshot snapshot, RigidBody rigidBody)
        {
            var force = GetMovementForce(snapshot.MovementForce);

            if (rigidBody.IsInTheAir())
            {
                force *= 0.5d;
            }
            else if (rigidBody.MomentumHasDifferentHorizontalDirectionFrom(force))
            {
                force *= 2d;
            }

            return force;
        }

        private static Vector GetMovementForce(MovementForce desiredMovementForce)
        {
            switch (desiredMovementForce)
            {
                case Other.MovementForce.Stop:
                    return Vector.Zero;
                case Other.MovementForce.Left:
                    return Vector.Left * MovementForce;
                case Other.MovementForce.Right:
                    return Vector.Right * MovementForce;
                default:
                    throw new ArgumentOutOfRangeException("desiredMovementForce");
            }
        }

        private static readonly Vector _jumpMomentum = Vector.XY(0, 1.5d);
        private const double MovementForce = 4d;

        private readonly IRepository<WorldId, World> _worldRepository;
        private readonly IRepository<RigidBodyId, RigidBody> _rigidBodyRepository;
    }
}
