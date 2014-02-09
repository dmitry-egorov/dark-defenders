using DarkDefenders.Domain.Data.Entities.Clocks;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Clocks.Events
{
    internal class ClockCreated : Created<Clock>
    {
        public ClockCreated(IStorage<Clock> storage) : base(storage)
        {
        }

        protected override object CreateData(IdentityOf<Clock> clockId)
        {
            return new ClockCreatedData(clockId);
        }

        protected override Clock Create()
        {
            return new Clock();
        }
    }
}