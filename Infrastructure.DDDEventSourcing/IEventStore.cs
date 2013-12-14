using System.Collections.Generic;
using Infrastructure.DDDEventSourcing.Domain;

namespace Infrastructure.DDDEventSourcing
{
    public interface IEventStore
    {
        IEnumerable<IEvent> Get(Identity id);

        void Append(Identity id, IEnumerable<IEvent> events);
    }
}