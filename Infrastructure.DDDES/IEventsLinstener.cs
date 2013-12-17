using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface IEventsLinstener
    {
        void Apply(IEnumerable<IEvent> events);
    }
}