using System.Collections.Generic;

namespace Infrastructure.DDDEventSourcing
{
    public interface ICommandHandler<in TCommand>
    {
        IEnumerable<IEvent> Handle(TCommand command);
    }
}