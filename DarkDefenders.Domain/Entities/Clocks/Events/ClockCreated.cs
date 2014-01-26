using DarkDefenders.Dtos.Entities.Clocks;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Entities.Clocks.Events
{
    internal class ClockCreated : Created<Clock, ClockId>
    {
        public ClockCreated(IStorage<Clock> storage) : base(storage)
        {
        }

        protected override object CreateDto(ClockId rootId)
        {
            return new ClockCreatedDto(rootId);
        }

        protected override Clock Create()
        {
            return new Clock();
        }
    }
}