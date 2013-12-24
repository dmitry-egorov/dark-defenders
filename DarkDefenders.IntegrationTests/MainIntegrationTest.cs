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
        private TestEventListener _eventListener;
        private ICommandProcessor _commandProcessor;

        [SetUp]
        public void SetUp()
        {
            _eventListener = new TestEventListener();
            _commandProcessor = CreateBus(_eventListener);
        }

        [Test]
        public void Should_create_player()
        {
            var spawnPosition = new Vector(0, 0);
            var boundingCircle = new Circle(spawnPosition, Player.BoundingCircleRadius);
            var mass = Player.Mass;

            var worldId = CreateWorld(spawnPosition);
            var playerId = CreatePlayer(worldId);
            var rigidBodyId = FindRigidBodyId();

            var expectedEvents = new IEvent[]
            {
                new WorldCreated(worldId, spawnPosition), 
                new RigidBodyCreated(rigidBodyId, worldId, boundingCircle, Vector.Zero, mass),
                new PlayerCreated(playerId, worldId, rigidBodyId)
            };

            _eventListener.AssertEvents(expectedEvents);
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_player()
        {
            var spawnPosition = new Vector(0, 0);
            var boundingCircle = new Circle(spawnPosition, Player.BoundingCircleRadius);
            var desiredOrientation = MovementForceDirection.Left;
            var mass = Player.Mass;

            var worldId = CreateWorld(spawnPosition);
            var playerId = CreatePlayer(worldId);
            var rigidBodyId = FindRigidBodyId();

            MovePlayerLeft(playerId);

            var expectedEvents = new IEvent[]
            {
                new WorldCreated(worldId, spawnPosition), 
                new RigidBodyCreated(rigidBodyId, worldId, boundingCircle, Vector.Zero, mass),
                new PlayerCreated(playerId, worldId, rigidBodyId),
                new MovementForceDirectionChanged(playerId, desiredOrientation), 
            };
            _eventListener.AssertEvents(expectedEvents);
        }

        private RigidBodyId FindRigidBodyId()
        {
            return _eventListener.FindEvents<RigidBodyCreated>().Single().RootId;
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_player_and_move_player_on_update()
        {
            var spawnPosition = new Vector(0, 0.025);
            var boundingCircle = new Circle(spawnPosition, Player.BoundingCircleRadius);
            var desiredOrientation = MovementForceDirection.Left;
            var externalForce = Vector.XY(-4.0, 0);
            var elapsed = TimeSpan.FromMilliseconds(20).TotalSeconds;
            var newMomentum = Vector.XY(-0.08, 0);
            var newPosition = Vector.XY(-0.0016, 0.025);
            var mass = Player.Mass;

            var worldId = CreateWorld(spawnPosition);
            var playerId = CreatePlayer(worldId);
            var rigidBodyId = FindRigidBodyId();

            MovePlayerLeft(playerId);
            UpdateAll(elapsed);

            var expectedEvents = new IEvent[]
            {
                new WorldCreated(worldId, spawnPosition), 
                new RigidBodyCreated(rigidBodyId, worldId, boundingCircle, Vector.Zero, mass),
                new PlayerCreated(playerId, worldId, rigidBodyId),
                new MovementForceDirectionChanged(playerId, desiredOrientation), 
                new WorldTimeUpdated(worldId, elapsed, elapsed), 
                new ExternalForceChanged(rigidBodyId, externalForce), 
                new Accelerated(rigidBodyId, newMomentum),
                new Moved(rigidBodyId, newPosition), 
            };

            _eventListener.AssertEvents(expectedEvents);
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
            var worldId = new WorldId();

            CreateWorld(worldId, spawnPosition);

            Assert.Throws<RootAlreadyExistsException>(() => CreateWorld(worldId, spawnPosition));
        }

        private void UpdateAll(double elapsed)
        {
            _commandProcessor.ProcessAllAndCommit<World>(root => root.UpdateWorldTime(elapsed));
            _commandProcessor.ProcessAllAndCommit<Player>(root => root.ApplyMovementForce());
            _commandProcessor.ProcessAllAndCommit<RigidBody>(root => root.UpdateMomentum());
            _commandProcessor.ProcessAllAndCommit<RigidBody>(root => root.UpdatePosition());
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
            _commandProcessor.CreateAndCommit<WorldFactory>(f => f.Create(worldId, spawnPosition));
        }

        private void CreatePlayer(PlayerId playerId, WorldId worldId)
        {
            _commandProcessor.CreateAndCommit<PlayerFactory>(f => f.Create(playerId, worldId));
        }

        private void MovePlayerLeft(PlayerId playerId)
        {
            _commandProcessor.ProcessAndCommit<Player>(playerId, player => player.MoveLeft());
        }

        private static ICommandProcessor CreateBus(TestEventListener testEventListener)
        {
            var processor = new CommandProcessor(testEventListener);

            processor.ConfigureDomain();

            return processor;
        }

        public class TestEventListener: IEventsLinstener
        {
            private readonly List<IEvent> _allEvents = new List<IEvent>();

            public void AssertEvents(IEnumerable<IEvent> expectedEvents)
            {
                CollectionAssert.AreEqual(expectedEvents.AsReadOnly(), _allEvents.AsReadOnly());
            }

            public void Recieve(IEnumerable<IEvent> events)
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
