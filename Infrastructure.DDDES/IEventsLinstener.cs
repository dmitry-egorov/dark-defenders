using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface IEventsLinstener
    {
        void Recieve(IEnumerable<IEvent> events);
    }
}