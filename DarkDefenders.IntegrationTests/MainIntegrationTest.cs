using System;
using System.Collections.Generic;
using System.Linq;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Events;
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
        private ICommandProcessor<IDomainEvent> _commandProcessor;

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
            var dimensions = new Dimensions(10, 10);
            var boundingCircle = new Circle(spawnPosition, Player.BoundingCircleRadius);
            var mass = Player.Mass;

            var worldId = CreateWorld(dimensions, spawnPosition);
            var playerId = CreatePlayer(worldId);
            var rigidBodyId = FindRigidBodyId();

            var expectedEvents = new IDomainEvent[]
            {
                new WorldCreated(worldId, dimensions, spawnPosition), 
                new RigidBodyCreated(rigidBodyId, worldId, boundingCircle, Vector.Zero, mass),
                new PlayerCreated(playerId, worldId, rigidBodyId)
            };

            _eventListener.AssertEvents(expectedEvents);
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_player()
        {
            var spawnPosition = new Vector(0, 0);
            var dimensions = new Dimensions(10, 10);
            var boundingCircle = new Circle(spawnPosition, Player.BoundingCircleRadius);
            var desiredOrientation = MovementForceDirection.Left;
            var mass = Player.Mass;

            var worldId = CreateWorld(dimensions, spawnPosition);
            var playerId = CreatePlayer(worldId);
            var rigidBodyId = FindRigidBodyId();

            MovePlayerLeft(playerId);

            var expectedEvents = new IDomainEvent[]
            {
                new WorldCreated(worldId, dimensions, spawnPosition), 
                new RigidBodyCreated(rigidBodyId, worldId, boundingCircle, Vector.Zero, mass),
                new PlayerCreated(playerId, worldId, rigidBodyId),
                new MovementForceDirectionChanged(playerId, desiredOrientation), 
            };
            _eventListener.AssertEvents(expectedEvents);
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_player_and_move_player_on_update()
        {
            var spawnPosition = new Vector(0, 0.025);
            var dimensions = new Dimensions(10, 10);
            var boundingCircle = new Circle(spawnPosition, Player.BoundingCircleRadius);
            var desiredOrientation = MovementForceDirection.Left;
            var externalForce = Vector.XY(-4.0, 0);
            var elapsed = TimeSpan.FromMilliseconds(20).TotalSeconds;
            var newMomentum = Vector.XY(-0.08, 0);
            var newPosition = Vector.XY(-0.0016, 0.025);
            var mass = Player.Mass;

            var worldId = CreateWorld(dimensions, spawnPosition);
            var playerId = CreatePlayer(worldId);
            var rigidBodyId = FindRigidBodyId();

            MovePlayerLeft(playerId);
            UpdateAll(elapsed);

            var expectedEvents = new IDomainEvent[]
            {
                new WorldCreated(worldId, dimensions, spawnPosition), 
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
            var dimensions = new Dimensions(10, 10);

            CreateWorld(worldId, dimensions, spawnPosition);

            Assert.Throws<RootAlreadyExistsException>(() => CreateWorld(worldId, dimensions, spawnPosition));
        }

        private RigidBodyId FindRigidBodyId()
        {
            return _eventListener.FindEvents<RigidBodyCreated>().Single().RootId;
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

        private WorldId CreateWorld(Dimensions dimensions, Vector spawnPosition)
        {
            var worldId = new WorldId();

            CreateWorld(worldId, dimensions, spawnPosition);

            return worldId;
        }

        private void CreateWorld(WorldId worldId, Dimensions dimensions, Vector spawnPosition)
        {
            _commandProcessor.CreateAndCommit<WorldFactory>(f => f.Create(worldId, dimensions, spawnPosition));
        }

        private void CreatePlayer(PlayerId playerId, WorldId worldId)
        {
            _commandProcessor.CreateAndCommit<PlayerFactory>(f => f.Create(playerId, worldId));
        }

        private void MovePlayerLeft(PlayerId playerId)
        {
            _commandProcessor.ProcessAndCommit<Player>(playerId, player => player.MoveLeft());
        }

        private static ICommandProcessor<IDomainEvent> CreateBus(TestEventListener testEventListener)
        {
            var processor = new CommandProcessor<IDomainEvent>(testEventListener);

            processor.ConfigureDomain();

            return processor;
        }

        private class TestEventListener : IEventsLinstener<IDomainEvent>
        {
            private readonly List<IDomainEvent> _allEvents = new List<IDomainEvent>();

            public void AssertEvents(IEnumerable<IDomainEvent> expectedEvents)
            {
                CollectionAssert.AreEqual(expectedEvents.AsReadOnly(), _allEvents.AsReadOnly());
            }

            public void Recieve(IEnumerable<IDomainEvent> events)
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
