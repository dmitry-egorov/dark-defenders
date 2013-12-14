using System.Collections.Generic;

namespace Infrastructure.DDDEventSourcing
{
    public interface ICommandProcessor<in TCommand>
    {
        IEnumerable<IEventMarker> Process(TCommand command);
    }
}