using System.Collections.Generic;
using Infrastructure.DDDEventSourcing.Domain;

namespace Infrastructure.DDDEventSourcing
{
    public interface IEventStore
    {
        IEnumerable<IEventMarker> Get(Identity id);

        void Append(Identity id, IEnumerable<IEventMarker> events);
    }
}