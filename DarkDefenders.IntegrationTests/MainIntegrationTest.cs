using System;
using System.Collections.Generic;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Players;
using DarkDefenders.Domain.Players.Events;
using DarkDefenders.Domain.Terrains;
using DarkDefenders.Domain.Terrains.Events;
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
        private IEventStore _eventStore;
        private IBus _bus;

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

            var terrainId = CreateTerrain(spawnPosition);
            var playerId = CreatePlayer(terrainId);

            var expectedEvents = new IEvent[]
            {
                new TerrainCreated(terrainId, spawnPosition), 
                new PlayerCreated(playerId, terrainId, spawnPosition)
            };

            AssertEvents(expectedEvents);
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_player()
        {
            var spawnPosition = new Vector(0, 0);
            var direction = MoveDirection.Right;
            var desiredOrientation = new Vector(1, 0);

            var terrainId = CreateTerrain(spawnPosition);
            var playerId = CreatePlayer(terrainId);

            SetPlayersDesiredOrientation(playerId, direction);

            var expectedEvents = new IEvent[]
            {
                new TerrainCreated(terrainId, spawnPosition), 
                new PlayerCreated(playerId, terrainId, spawnPosition),
                new PlayersDesiredOrientationIsSet(playerId, desiredOrientation), 
            };

            AssertEvents(expectedEvents);
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_player_and_move_player_on_update()
        {
            var spawnPosition = new Vector(0, 0);
            var direction = MoveDirection.Right;
            var desiredOrientation = new Vector(1, 0);
            var elapsed = TimeSpan.FromMilliseconds(20);
            var newPosition = new Vector(0.02, 0);

            var terrainId = CreateTerrain(spawnPosition);
            var playerId = CreatePlayer(terrainId);

            SetPlayersDesiredOrientation(playerId, direction);
            UpdateAll(elapsed);

            var expectedEvents = new IEvent[]
            {
                new TerrainCreated(terrainId, spawnPosition), 
                new PlayerCreated(playerId, terrainId, spawnPosition),
                new PlayersDesiredOrientationIsSet(playerId, desiredOrientation), 
                new PlayerMoved(playerId, newPosition)
            };

            AssertEvents(expectedEvents);
        }

        [Test]
        public void Should_throw_when_terrain_is_not_created()
        {
            var fakeTerrainId = new TerrainId();
            var playerId = new PlayerId();

            Assert.Throws<RootDoesntExistException>(() => CreatePlayer(playerId, fakeTerrainId));
        }

        [Test]
        public void Should_throw_when_terrain_is_created_twice()
        {
            var spawnPosition = new Vector(0, 0);
            var terrainId = CreateTerrain(spawnPosition);

            Assert.Throws<RootAlreadyExistsException>(() => CreateTerrain(terrainId, spawnPosition));
        }

        private void UpdateAll(TimeSpan elapsed)
        {
            _bus.PublishToAllOfType<IUpdateable>(root => root.Update(elapsed));
        }

        private PlayerId CreatePlayer(TerrainId terrainId)
        {
            var playerId = new PlayerId();

            CreatePlayer(playerId, terrainId);

            return playerId;
        }

        private TerrainId CreateTerrain(Vector spawnPosition)
        {
            var terrainId = new TerrainId();

            CreateTerrain(terrainId, spawnPosition);

            return terrainId;
        }

        private void CreateTerrain(TerrainId terrainId, Vector spawnPosition)
        {
            _bus.PublishTo<Terrain>(terrainId, terrain => terrain.Create(terrainId, spawnPosition));
        }

        private void CreatePlayer(PlayerId playerId, TerrainId terrainId)
        {
            _bus.PublishTo<Player>(playerId, player => player.Create(playerId, terrainId));
        }

        private void SetPlayersDesiredOrientation(PlayerId playerId, MoveDirection direction)
        {
            _bus.PublishTo<Player>(playerId, player => player.Move(direction));
        }

        private void AssertEvents(IEnumerable<IEvent> expectedEvents)
        {
            var actualEvents = _eventStore.GetAll().AsReadOnly();

            CollectionAssert.AreEqual(expectedEvents.AsReadOnly(), actualEvents);
        }

        private static IBus CreateBus(IEventStore eventStore)
        {
            var processor = new CommandProcessor();

            processor.ConfigureDomain(eventStore);

            var bus = new Bus(processor);

            bus.Subscribe(eventStore.Append);

            return bus;
        }
    }
}
