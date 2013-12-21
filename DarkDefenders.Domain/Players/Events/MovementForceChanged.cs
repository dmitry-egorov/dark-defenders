using DarkDefenders.Domain.Other;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Players.Events
{
    public class MovementForceChanged: EventBase<PlayerId, MovementForceChanged>, IPlayerEvent
    {
        public MovementForceDirection MovementForceDirection { get; private set; }

        public MovementForceChanged(PlayerId rootId, MovementForceDirection movementForceDirection) : base(rootId)
        {
            MovementForceDirection = movementForceDirection;
        }

        protected override string ToStringInternal()
        {
            return "Movement force changed: {0}, {1}".FormatWith(RootId, MovementForceDirection);
        }

        protected override bool EventEquals(MovementForceChanged other)
        {
            return MovementForceDirection.Equals(other.MovementForceDirection);
        }

        protected override int GetEventHashCode()
        {
            return MovementForceDirection.GetHashCode();
        }

        public void ApplyTo(IPlayerEventsReciever reciever)
        {
            reciever.Apply(this);
        }
    }
}