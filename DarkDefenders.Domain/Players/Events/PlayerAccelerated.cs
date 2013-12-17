using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Players.Events
{
    public class PlayerAccelerated : EventBase<PlayerId, PlayerAccelerated>, IPlayerEvent
    {
        public Vector NewMomentum { get; private set; }

        public PlayerAccelerated(PlayerId id, Vector newMomentum)
            : base(id)
        {
            NewMomentum = newMomentum;
        }

        protected override string EventToString()
        {
            return "Player moved: {0} {1}".FormatWith(RootId, NewMomentum);
        }

        protected override bool EventEquals(PlayerAccelerated other)
        {
            return NewMomentum == other.NewMomentum;
        }

        protected override int GetEventHashCode()
        {
            return NewMomentum.GetHashCode();
        }

        public void ApplyTo(IPlayerEventsReciever reciever)
        {
            reciever.Apply(this);
        }
    }
}