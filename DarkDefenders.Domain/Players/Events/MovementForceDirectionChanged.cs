using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Other;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Players.Events
{
    public class MovementForceDirectionChanged: EventBase<PlayerId, MovementForceDirectionChanged>, IPlayerEvent
    {
        public MovementForceDirection MovementForceDirection { get; private set; }

        public MovementForceDirectionChanged(PlayerId rootId, MovementForceDirection movementForceDirection) : base(rootId)
        {
            MovementForceDirection = movementForceDirection;
        }

        protected override string ToStringInternal()
        {
            return "Movement force changed: {0}, {1}".FormatWith(RootId, MovementForceDirection);
        }

        protected override bool EventEquals(MovementForceDirectionChanged other)
        {
            return MovementForceDirection.Equals(other.MovementForceDirection);
        }

        protected override int GetEventHashCode()
        {
            return MovementForceDirection.GetHashCode();
        }

        public void ApplyTo(IPlayerEventsReciever reciever)
        {
            reciever.Recieve(this);
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}