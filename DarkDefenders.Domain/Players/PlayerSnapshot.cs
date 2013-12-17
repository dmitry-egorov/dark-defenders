using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.Worlds;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Players
{
    public class PlayerSnapshot : IPlayerEventsReciever
    {
        public WorldId WorldId { get; private set; }
        public Vector Position { get; private set; }
        public Vector Momentum { get; private set; }
        public MovementForce MovementForce { get; private set; }
        
        public void Apply(PlayerCreated playerCreated)
        {
            WorldId = playerCreated.WorldId;
            Position = playerCreated.SpawnPosition;
            MovementForce = MovementForce.Stop;
            Momentum = Vector.Zero;
        }

        public void Apply(MovementForceChanged movementForceChanged)
        {
            MovementForce = movementForceChanged.MovementForce;
        }

        public void Apply(PlayerMoved playerMoved)
        {
            Position = playerMoved.NewPosition;
        }

        public void Apply(PlayerAccelerated playerAccelerated)
        {
            Momentum = playerAccelerated.NewMomentum;
        }
    }
}