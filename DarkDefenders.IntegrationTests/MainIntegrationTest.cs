using System;
using System.Collections.Generic;
using System.Linq;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Players;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Worlds;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations;
using Infrastructure.DDDES.Implementations.Domain.Exceptions;
using Infrastructure.Math;
using Infrastructure.Util;
using NUnit.Framework;

namespace DarkDefenders.IntegrationTests
{
    [TestFixture]
    public class MainIntegrationTest
    {
        private EventStore _eventStore;
        private ICommandProcessor _bus;

        [SetUp]
        public void SetUp()
        {
            _eventStore = new EventStore();
            _bus = CreateBus(_eventStore);
        }

        [Test]
        public void Should_create_player()
        {
            var spawnPosition = new Vector(0, 0);

            var worldId = CreateWorld(spawnPosition);
            var playerId = CreatePlayer(worldId);
            var rigidBodyId = FindRigidBodyId();

            var expectedEvents = new IEvent[]
            {
                new WorldCreated(worldId, spawnPosition), 
                new RigidBodyCreated(rigidBodyId, worldId, spawnPosition),
                new PlayerCreated(playerId, worldId, rigidBodyId)
            };

            _eventStore.AssertEvents(expectedEvents);
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_player()
        {
            var spawnPosition = new Vector(0, 0);
            var desiredOrientation = MovementForce.Left;

            var worldId = CreateWorld(spawnPosition);
            var playerId = CreatePlayer(worldId);
            var rigidBodyId = FindRigidBodyId();

            MovePlayerLeft(playerId);

            var expectedEvents = new IEvent[]
            {
                new WorldCreated(worldId, spawnPosition), 
                new RigidBodyCreated(rigidBodyId, worldId, spawnPosition),
                new PlayerCreated(playerId, worldId, rigidBodyId),
                new MovementForceChanged(playerId, desiredOrientation), 
            };
            _eventStore.AssertEvents(expectedEvents);
        }

        private RigidBodyId FindRigidBodyId()
        {
            return _eventStore.FindEvents<RigidBodyCreated>().Single().RootId;
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_player_and_move_player_on_update()
        {
            var spawnPosition = new Vector(0, 0);
            var desiredOrientation = MovementForce.Left;
            var externalForce = Vector.XY(-8.0, 0);
            var elapsed = TimeSpan.FromMilliseconds(20);
            var newMomentum = new Vector(-0.16, 0);

            var worldId = CreateWorld(spawnPosition);
            var playerId = CreatePlayer(worldId);
            var rigidBodyId = FindRigidBodyId();

            MovePlayerLeft(playerId);
            UpdateAll(elapsed);

            var expectedEvents = new IEvent[]
            {
                new WorldCreated(worldId, spawnPosition), 
                new RigidBodyCreated(rigidBodyId, worldId, spawnPosition),
                new PlayerCreated(playerId, worldId, rigidBodyId),
                new MovementForceChanged(playerId, desiredOrientation), 
                new ExternalForceChanged(rigidBodyId, externalForce), 
                new Accelerated(rigidBodyId, newMomentum)
            };

            _eventStore.AssertEvents(expectedEvents);
        }

        [Test]
        public void Should_throw_when_world_is_not_created()
        {
            var fakeWorldId = new WorldId();
            var playerId = new PlayerId();

            Assert.Throws<RootDoesntExistException>(() => CreatePlayer(playerId, fakeWorldId));
        }

        [Test]
        public void Should_throw_when_world_is_created_twice()
        {
            var spawnPosition = new Vector(0, 0);
            var worldId = CreateWorld(spawnPosition);

            Assert.Throws<RootAlreadyExistsException>(() => CreateWorld(worldId, spawnPosition));
        }

        private void UpdateAll(TimeSpan elapsed)
        {
            _bus.ProcessAllAndCommit<Player>(root => root.ApplyMovementForce());
            _bus.ProcessAllAndCommit<RigidBody>(root => root.UpdateKineticState(elapsed));
        }

        private PlayerId CreatePlayer(WorldId worldId)
        {
            var playerId = new PlayerId();

            CreatePlayer(playerId, worldId);

            return playerId;
        }

        private WorldId CreateWorld(Vector spawnPosition)
        {
            var worldId = new WorldId();

            CreateWorld(worldId, spawnPosition);

            return worldId;
        }

        private void CreateWorld(WorldId worldId, Vector spawnPosition)
        {
            _bus.CreateAndCommit<World>(worldId, world => world.Create(spawnPosition));
        }

        private void CreatePlayer(PlayerId playerId, WorldId worldId)
        {
            _bus.CreateAndCommit<Player>(playerId, player => player.Create(worldId));
        }

        private void MovePlayerLeft(PlayerId playerId)
        {
            _bus.ProcessAndCommit<Player>(playerId, player => player.MoveLeft());
        }

        private static ICommandProcessor CreateBus(EventStore eventStore)
        {
            var processor = new CommandProcessor(eventStore);

            processor.ConfigureDomain();

            return processor;
        }

        public class EventStore: IEventsLinstener
        {
            private readonly List<IEvent> _allEvents = new List<IEvent>();

            public void AssertEvents(IEnumerable<IEvent> expectedEvents)
            {
                CollectionAssert.AreEqual(expectedEvents.AsReadOnly(), _allEvents.AsReadOnly());
            }

            public void Apply(IEnumerable<IEvent> events)
            {
                var readOnly = events.AsReadOnly();

                _allEvents.AddRange(readOnly);
            }

            public IEnumerable<TEvent> FindEvents<TEvent>()
            {
                return _allEvents.OfType<TEvent>();
            }
        }
    }
}
