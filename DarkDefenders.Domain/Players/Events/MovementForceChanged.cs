using DarkDefenders.Domain.Other;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Players.Events
{
    public class MovementForceChanged : EventBase<PlayerId, MovementForceChanged>, IPlayerEvent
    {
        public MovementForce MovementForce { get; private set; }

        public MovementForceChanged(PlayerId playerId, MovementForce movementForce) : base(playerId)
        {
            MovementForce = movementForce;
        }

        protected override string EventToString()
        {
            return "Players movement force changed: {0}, {1}".FormatWith(RootId, MovementForce);
        }

        protected override bool EventEquals(MovementForceChanged other)
        {
            return MovementForce == other.MovementForce;
        }

        protected override int GetEventHashCode()
        {
            return MovementForce.GetHashCode();
        }

        public void ApplyTo(IPlayerEventsReciever reciever)
        {
            reciever.Apply(this);
        }
    }
}