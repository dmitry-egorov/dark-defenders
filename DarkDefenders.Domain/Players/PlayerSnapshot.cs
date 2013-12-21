using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;

namespace DarkDefenders.Domain.Players
{
    public class PlayerSnapshot : IPlayerEventsReciever
    {
        public WorldId WorldId { get; private set; }
        public MovementForceDirection MovementForceDirection { get; private set; }
        public Direction Direction { get; private set; }
        public RigidBodyId RigidBodyId { get; private set; }

        public void Apply(PlayerCreated playerCreated)
        {
            WorldId = playerCreated.WorldId;
            RigidBodyId = playerCreated.RigidBodyId;
            MovementForceDirection = MovementForceDirection.Stop;
            Direction = Direction.Right;
        }

        public void Apply(MovementForceChanged movementForceChanged)
        {
            var movementForce = movementForceChanged.MovementForceDirection;
            MovementForceDirection = movementForce;
            Direction = GetDirectionFrom(movementForce);
        }

        private Direction GetDirectionFrom(MovementForceDirection movementForceDirection)
        {
            switch (movementForceDirection)
            {
                case MovementForceDirection.Left:
                    return Direction.Left;
                case MovementForceDirection.Right:
                    return Direction.Right;
                default:
                    return Direction;
            }
        }
    }
}