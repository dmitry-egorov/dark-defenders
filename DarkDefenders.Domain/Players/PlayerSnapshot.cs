using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;

namespace DarkDefenders.Domain.Players
{
    public class PlayerSnapshot : IPlayerEventsReciever
    {
        public WorldId WorldId { get; private set; }
        public MovementForce MovementForce { get; private set; }
        public Direction Direction { get; private set; }
        public RigidBodyId RigidBodyId { get; private set; }

        public void Apply(PlayerCreated playerCreated)
        {
            WorldId = playerCreated.WorldId;
            RigidBodyId = playerCreated.RigidBodyId;
            MovementForce = MovementForce.Stop;
            Direction = Direction.Right;
        }

        public void Apply(MovementForceChanged movementForceChanged)
        {
            var movementForce = movementForceChanged.MovementForce;
            MovementForce = movementForce;
            Direction = GetDirectionFrom(movementForce);
        }

        private Direction GetDirectionFrom(MovementForce movementForce)
        {
            switch (movementForce)
            {
                case MovementForce.Left:
                    return Direction.Left;
                case MovementForce.Right:
                    return Direction.Right;
                default:
                    return Direction;
            }
        }
    }
}