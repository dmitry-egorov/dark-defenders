using DarkDefenders.Domain.Events;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.Domain.Worlds.Events
{
    public class WorldTimeUpdated : EventBase<WorldId, WorldTimeUpdated>, IWorldEvent
    {
        public double NewTime { get; private set; }
        public double Elapsed { get; private set; }

        public WorldTimeUpdated(WorldId worldId, double newTime, double elapsed): base(worldId)
        {
            NewTime = newTime;
            Elapsed = elapsed;
        }

        protected override string ToStringInternal()
        {
            return "World time updated: {0}, {1}".FormatWith(RootId, NewTime);
        }

        protected override bool EventEquals(WorldTimeUpdated other)
        {
            return NewTime.Equals(other.NewTime);
        }

        protected override int GetEventHashCode()
        {
            return NewTime.GetHashCode();
        }

        public void ApplyTo(IWorldEventsReciever reciever)
        {
            reciever.Recieve(this);
        }

        public void Accept(IDomainEventReciever reciever)
        {
            reciever.Recieve(this);
        }
    }
}