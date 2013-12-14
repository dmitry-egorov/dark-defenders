using DarkDefenders.Domain.Player;
using DarkDefenders.Domain.Player.Command;
using Infrastructure.DDDEventSourcing;
using Infrastructure.DDDEventSourcing.Implementations;

namespace DarkDefenders.Domain
{
    public static class Configurator
    {
        public static void ConfigureDomain(this Bus bus, IEventStore eventStore)
        {
            var playerRepository = new Repository(eventStore);

            bus.AddRepositoryFor<Create, AggregateRoot, Id>(playerRepository);
        }
    }
}