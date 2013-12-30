using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface IEventsLinstener<in TDomainEvent>
    {
        void Recieve(IEnumerable<TDomainEvent> events);
    }
}