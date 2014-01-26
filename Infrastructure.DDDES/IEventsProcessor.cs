using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface IEventsProcessor
    {
        void Process(IEnumerable<IEvent> events);
    }
}