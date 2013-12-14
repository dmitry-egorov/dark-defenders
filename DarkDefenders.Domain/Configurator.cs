using DarkDefenders.Domain.Player;
using DarkDefenders.Domain.Player.Command;
using DarkDefenders.Domain.Player.Event;
using Infrastructure.DDDEventSourcing;
using Infrastructure.DDDEventSourcing.Implementations;
using Infrastructure.DDDEventSourcing.Implementations.Domain;

namespace DarkDefenders.Domain
{
    public static class Configurator
    {
        public static void ConfigureDomain(this CommandProcessor processor, IEventStore eventStore)
        {
            var playerRepository = new Repository<Root, RootState, IEvent, IEventReciever, Id>(eventStore);

            processor.AddHandlerFor<Create, Root, Id>(playerRepository);
        }
    }
}