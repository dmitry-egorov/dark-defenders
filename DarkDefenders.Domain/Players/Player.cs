using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Players
{
    public class Player : RootBase<PlayerId, PlayerSnapshot, IPlayerEventsReciever, IPlayerEvent>, IUpdateable
    {
        private readonly IRepository<WorldId, World> _worldRepository;
        private static readonly Vector _jumpMomentum = Vector.XY(0, 1.5d);
        private const double BoundingCircleRaius = 1d / 40d;
        private const double TopHorizontalMomentum = 0.8d;
        private const double MovementForce = 4d;
        private const double Mass = 1d;
        private const double InverseMass = 1d / Mass;

        internal Player(PlayerId id, IRepository<WorldId, World> worldRepository) : base(id)
        {
            _worldRepository = worldRepository;
        }

        public IEnumerable<IPlayerEvent> Create(WorldId worldId)
        {
            AssertDoesntExist();

            var world = _worldRepository.GetById(worldId);

            var spawnPosition = world.GetSpawnPosition();

            yield return new PlayerCreated(Id, worldId, spawnPosition);
        }

        public IEnumerable<IEvent> Stop()
        {
            return SetMovementForce(Other.MovementForce.Stop);
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
            var world = _worldRepository.GetById(snapshot.WorldId);

            if (world.IsInTheAir(snapshot.Position, BoundingCircleRaius) 
                || HasVerticalMomentum(snapshot))
            {
                yield break;
            }

            var newMomentum = Snapshot.Momentum + _jumpMomentum;

            yield return new PlayerAccelerated(Id, newMomentum);
        }

        public IEnumerable<IEvent> Update(TimeSpan elapsed)
        {
            var snapshot = Snapshot;

            var world = _worldRepository.GetById(snapshot.WorldId);
            
            foreach (var @event in ApplyMomentum(elapsed, snapshot, world)) yield return @event;
            foreach (var @event in ApplyForce(elapsed, snapshot, world)) yield return @event;
        }

        private IEnumerable<IEvent> ApplyForce(TimeSpan elapsed, PlayerSnapshot snapshot, World world)
        {
            var momentum = snapshot.Momentum;
            var position = snapshot.Position;

            var newMomentum = ApplyMovementForce(elapsed, snapshot, world);

            newMomentum = world.ApplyGravityForce(position, BoundingCircleRaius, newMomentum, Mass, elapsed);
            if (snapshot.MovementForce == Other.MovementForce.Stop)
            {
                newMomentum = world.ApplyFrictionForce(position, BoundingCircleRaius, newMomentum, Mass, elapsed);
            }
            newMomentum = LimitMomentum(newMomentum);

            newMomentum = world.ApplyInelasticTerrainImpact(position, BoundingCircleRaius, newMomentum);

            if (newMomentum != momentum)
            {
                yield return new PlayerAccelerated(Id, newMomentum);
            }
        }

        private IEnumerable<IEvent> ApplyMomentum(TimeSpan elapsed, PlayerSnapshot snapshot, World world)
        {
            var momentum = snapshot.Momentum;
            var position = snapshot.Position;

            if (momentum == Vector.Zero)
            {
                yield break;
            }

            var positionChange = momentum * elapsed.TotalSeconds * InverseMass;

            var newPosition = position + positionChange;

            newPosition = world.AdjustPosition(newPosition, BoundingCircleRaius);

            yield return new PlayerMoved(Id, newPosition);
        }

        private static Vector ApplyMovementForce(TimeSpan elapsed, PlayerSnapshot snapshot, World world)
        {
            var momentum = snapshot.Momentum;
            var position = snapshot.Position;

            var force = GetMovementForce(snapshot.MovementForce);

            if (world.IsInTheAir(position, BoundingCircleRaius))
            {
                force *= 0.5d;
            }

            force *= elapsed.TotalSeconds;

            return momentum + force;
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

        private static Vector LimitMomentum(Vector momentum)
        {
            var vx = momentum.X;
            var vy = momentum.Y;

            return Math.Abs(vx) > TopHorizontalMomentum 
                   ? Vector.XY(Math.Sign(vx) * TopHorizontalMomentum, vy) 
                   : momentum;
        }

        private IEnumerable<IEvent> SetMovementForce(MovementForce movementForce)
        {
            if (Snapshot.MovementForce != movementForce)
            {
                yield return new MovementForceChanged(Id, movementForce);
            }
        }

        private static bool HasVerticalMomentum(PlayerSnapshot snapshot)
        {
            return Math.Abs(snapshot.Momentum.Y) > 0.01d;
        }
    }
}
