using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface IEventStore
    {
        IEnumerable<IEvent> GetAll();

        void Append(IEnumerable<IEvent> events);
    }
}