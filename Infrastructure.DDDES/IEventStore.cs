using System.Collections.Generic;

namespace Infrastructure.DDDES
{
    public interface IEventStore
    {
        IEnumerable<IEvent> GetById(Identity id);

        IEnumerable<IEvent> GetAll();

        void Append(IEvent e);

        void Append(IEnumerable<IEvent> events);

        IEnumerable<Identity> GetAllIds();
    }
}