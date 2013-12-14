using System.Collections.Generic;
using DarkDefenders.Domain.Player.Command;
using DarkDefenders.Domain.Player.Event;
using DarkDefenders.Domain.Player.Exception;
using Infrastructure.DDDEventSourcing;
using Infrastructure.DDDEventSourcing.Implementations.Domain;

namespace DarkDefenders.Domain.Player
{
    internal class Root: RootBase<RootState>, ICommandProcessor<Create>
    {
        public IEnumerable<IEventMarker> Process(Create command)
        {
            if (State.RootId != null)
            {
                throw new AlreadyCreatedException(command.RootId);
            }

            yield return new Created(command.RootId);
        }

    }
}
