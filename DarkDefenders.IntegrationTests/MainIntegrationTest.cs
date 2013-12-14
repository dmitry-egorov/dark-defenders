using System;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Player;
using DarkDefenders.Domain.Player.Command;
using DarkDefenders.Domain.Player.Event;
using Infrastructure.DDDEventSourcing;
using Infrastructure.DDDEventSourcing.Implementations;
using Infrastructure.Util;
using NUnit.Framework;

namespace DarkDefenders.IntegrationTests
{
    [TestFixture]
    public class MainIntegrationTest
    {
        private readonly IEventStore _eventStore;
        private readonly ICommandPublisher _commandPublisher;

        public MainIntegrationTest()
        {
            _eventStore = new EventStore();
            _commandPublisher = CreateBus(_eventStore);
        }

        [Test]
        public void Should_work()
        {
            var playerId = CreatePlayerId();

            _commandPublisher.Publish(new Create(playerId));

            var events = _eventStore.Get(playerId).AsReadOnly();

            var expectedEvents = new[] {new Created(playerId)}.AsReadOnly();

            CollectionAssert.AreEqual(expectedEvents, events);
        }

        private static Id CreatePlayerId()
        {
            return new Id(Guid.NewGuid());
        }

        private static ICommandPublisher CreateBus(IEventStore eventStore)
        {
            var bus = new Bus(eventStore);

            bus.ConfigureDomain(eventStore);

            return bus;
        }
    }
}
