using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
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
            //_commandPublisher.Publish<Root>(playerId, x => x.Create(playerId));

            AssertEvents(playerId, new[] { new Created(playerId) });
        }

        private void AssertEvents(Id playerId, IEnumerable<Created> expectedEvents)
        {
            var actualEvents = _eventStore.Get(playerId).AsReadOnly();

            CollectionAssert.AreEqual(expectedEvents.AsReadOnly(), actualEvents);
        }

        private static Id CreatePlayerId()
        {
            return new Id(Guid.NewGuid());
        }

        private static ICommandPublisher CreateBus(IEventStore eventStore)
        {
            var processor = new CommandProcessor();

            processor.ConfigureDomain(eventStore);

            return new Bus(processor, eventStore);
        }
    }
}
