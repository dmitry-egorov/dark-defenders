using Infrastructure.DDDEventSourcing;
using Infrastructure.DDDEventSourcing.Domain;

namespace DarkDefenders.Domain.Player
{
    internal class Repository : IRepository<AggregateRoot, Id>
    {
        private readonly IEventStore _eventStore;

        public Repository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public AggregateRoot GetById(Id id)
        {
            var events = _eventStore.Get(id);

            return new AggregateRoot(events);
        }
    }
}