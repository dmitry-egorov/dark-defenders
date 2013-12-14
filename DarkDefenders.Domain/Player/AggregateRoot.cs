using System.Collections.Generic;
using DarkDefenders.Domain.Player.Command;
using DarkDefenders.Domain.Player.Event;
using DarkDefenders.Domain.Player.Exception;
using Infrastructure.DDDEventSourcing;
using Infrastructure.DDDEventSourcing.Implementations;

namespace DarkDefenders.Domain.Player
{
    public class AggregateRoot : AggregateRootBase<State>, ICommandHandler<Create>
    {
        public AggregateRoot(IEnumerable<IEvent> events) : base(events) { }

        public IEnumerable<IEvent> Handle(Create command)
        {
            if (State.Id != null)
            {
                throw new AlreadyCreatedException(command.AggregateRootId);
            }

            yield return new Created(command.AggregateRootId);
        }

    }
}
