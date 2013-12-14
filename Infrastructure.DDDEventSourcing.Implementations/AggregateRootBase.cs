using System.Collections.Generic;
using Infrastructure.DDDEventSourcing.Domain;
using MoreLinq;

namespace Infrastructure.DDDEventSourcing.Implementations
{
    public abstract class AggregateRootBase<TState>: IAggregateRoot<TState>
        where TState: new()
    {
        public TState State { get; private set; }

        public AggregateRootBase(IEnumerable<IEvent> events)
        {
            State = new TState();
            events.ForEach(@event => ((dynamic)State).Apply((dynamic)@event));
        }
    }
}