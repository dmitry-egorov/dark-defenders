using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Clocks.Events
{
    internal class ClockCreated : Created<Clock>
    {
        public ClockCreated(Clock clock, IStorage<Clock> storage) : base(clock, storage)
        {
        }

        protected override void ApplyTo(Clock entity)
        {
            entity.Created();
        }

        public override void Accept(IEventsReciever reciever)
        {
        }
    }
}