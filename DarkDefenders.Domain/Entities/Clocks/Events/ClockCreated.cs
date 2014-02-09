using DarkDefenders.Domain.Infrastructure;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Clocks.Events
{
    internal class ClockCreated : Created<Clock>
    {
        public ClockCreated(IStorage<Clock> storage) : base(storage)
        {
        }

        protected override Clock Create()
        {
            return new Clock();
        }

        protected override void Accept(IEventsReciever reciever, IdentityOf<Clock> id)
        {
            
        }
    }
}