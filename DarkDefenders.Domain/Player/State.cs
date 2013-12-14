using DarkDefenders.Domain.Player.Event;
using Infrastructure.DDDEventSourcing.Domain;
using JetBrains.Annotations;

namespace DarkDefenders.Domain.Player
{
    public class State: IAggregateRootState<Id>
    {
        public Id Id { get; private set; }

        [UsedImplicitly]
        public void Apply(Created created)
        {
            Id = created.AggregateRootId;
        }
    }
}