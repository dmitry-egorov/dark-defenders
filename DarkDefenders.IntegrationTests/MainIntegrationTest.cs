using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Clocks.Events;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Creatures.Events;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.RigidBodies.Events;
using DarkDefenders.Domain.Terrains;
using DarkDefenders.Domain.Terrains.Events;
using DarkDefenders.Domain.Worlds;
using DarkDefenders.Domain.Worlds.Events;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations;
using Infrastructure.DDDES.Implementations.Domain.Exceptions;
using Infrastructure.Math;
using Infrastructure.Physics;
using Infrastructure.Util;
using NUnit.Framework;

namespace DarkDefenders.IntegrationTests
{
    [TestFixture]
    public class MainIntegrationTest
    {
        private static readonly RigidBodyProperties _playersRigidBodyProperties = new RigidBodyProperties(0.4, 1.0, 40.0);
        private static readonly CreatureProperties _playerProperties = new CreatureProperties(180.0, 60.0, _playersRigidBodyProperties);

        private static readonly RigidBodyProperties _heroesRigidBodyProperties = new RigidBodyProperties(0.4, 1.0, 40.0);
        private static readonly CreatureProperties _heroesProperties = new CreatureProperties(180.0, 60.0, _heroesRigidBodyProperties);

        private TestEventListener _eventListener;
        private ICommandProcessor<IDomainEvent> _commandProcessor;

        [SetUp]
        public void SetUp()
        {
            _eventListener = new TestEventListener();
            _commandProcessor = CreateBus(_eventListener);
        }

        [Test]
        public void Should_create_creature()
        {
            var spawnPosition = new Vector(0, 0);
            var spawnPositions = spawnPosition.EnumerateOnce().AsReadOnly();
            var dimensions = new Dimensions(10, 10);
            var map = new Map<Tile>(dimensions, default(Tile));

            var clockId = CreateClock();
            var terrainId = CreateTerrain(map);
            var worldId = CreateWorld(spawnPositions, clockId, terrainId);
            var avatarId = CreateAvatar(worldId);
            var rigidBodyId = FindRigidBodyId();

            var expectedEvents = new IDomainEvent[]
            {
                new ClockCreated(clockId), 
                new TerrainCreated(terrainId, map), 
                new WorldCreated(worldId, clockId, terrainId, spawnPositions, _playerProperties, spawnPositions, TimeSpan.FromSeconds(1), _heroesProperties), 
                new RigidBodyCreated(rigidBodyId, clockId, terrainId, spawnPosition, Momentum.Zero, _playersRigidBodyProperties),
                new CreatureCreated(avatarId, clockId, terrainId, rigidBodyId, _playerProperties),
                new PlayerAvatarSpawned(worldId, avatarId)
            };

            _eventListener.AssertEvents(expectedEvents);
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_creature()
        {
            var spawnPosition = new Vector(0, 0);
            var spawnPositions = spawnPosition.EnumerateOnce().AsReadOnly();
            var dimensions = new Dimensions(10, 10);
            var map = new Map<Tile>(dimensions, default(Tile));
            var desiredOrientation = Movement.Left;
            var externalForce = Vector.XY(-180, 0).ToForce();

            var clockId = CreateClock();
            var terrainId = CreateTerrain(map);
            var worldId = CreateWorld(spawnPositions, clockId, terrainId);
            var avatarId = CreateAvatar(worldId);
            var rigidBodyId = FindRigidBodyId();

            MoveCreatureLeft(avatarId);

            var expectedEvents = new IDomainEvent[]
            {
                new ClockCreated(clockId), 
                new TerrainCreated(terrainId, map), 
                new WorldCreated(worldId, clockId, terrainId, spawnPositions, _playerProperties, spawnPositions, TimeSpan.FromSeconds(1), _heroesProperties), 
                new RigidBodyCreated(rigidBodyId, clockId, terrainId, spawnPosition, Momentum.Zero, _playersRigidBodyProperties),
                new CreatureCreated(avatarId, clockId, terrainId, rigidBodyId, _playerProperties),
                new PlayerAvatarSpawned(worldId, avatarId),
                new MovementChanged(avatarId, desiredOrientation), 
                new ExternalForceChanged(rigidBodyId, externalForce), 
            };

            _eventListener.AssertEvents(expectedEvents);
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_creature_and_move_creature_on_update()
        {
            var spawnPosition = new Vector(50, 0.4);
            var spawnPositions = spawnPosition.EnumerateOnce().AsReadOnly();
            var dimensions = new Dimensions(100, 100);
            var map = new Map<Tile>(dimensions, default(Tile));
            map.Fill(Tile.Open);
            var desiredOrientation = Movement.Left;
            var elapsed = TimeSpan.FromMilliseconds(20);

            var externalForce = Vector.XY(-180, 0).ToForce();
            var newMomentum = Vector.XY(-3.6, 0).ToMomentum();
            var newPosition = Vector.XY(49.928, 0.4);

            var clockId = CreateClock();
            var terrainId = CreateTerrain(map);
            var worldId = CreateWorld(spawnPositions, clockId, terrainId);
            var avatarId = CreateAvatar(worldId);
            var rigidBodyId = FindRigidBodyId();

            MoveCreatureLeft(avatarId);
            UpdateAll(elapsed);

            var expectedEvents = new IDomainEvent[]
            {
                new ClockCreated(clockId), 
                new TerrainCreated(terrainId, map), 
                new WorldCreated(worldId, clockId, terrainId, spawnPositions, _playerProperties, spawnPositions, TimeSpan.FromSeconds(1), _heroesProperties), 
                new RigidBodyCreated(rigidBodyId, clockId, terrainId, spawnPosition, Momentum.Zero, _playersRigidBodyProperties),
                new CreatureCreated(avatarId, clockId, terrainId, rigidBodyId, _playerProperties),
                new PlayerAvatarSpawned(worldId, avatarId), 
                new MovementChanged(avatarId, desiredOrientation),
                new ExternalForceChanged(rigidBodyId, externalForce), 
                new ClockTimeUpdated(clockId, elapsed), 
                new Accelerated(rigidBodyId, newMomentum),
                new Moved(rigidBodyId, newPosition), 
            };

            _eventListener.AssertEvents(expectedEvents);
        }

        [Test]
        public void Should_throw_when_world_is_not_created()
        {
            var fakeWorldId = new WorldId();
            var creatureId = new CreatureId();

            Assert.Throws<RootDoesntExistException>(() => CreateAvatar(creatureId, fakeWorldId));
        }

        [Test]
        public void Should_throw_when_clock_is_created_twice()
        {
            var clockId = new ClockId();

            CreateClock(clockId);

            Assert.Throws<RootAlreadyExistsException>(() => CreateClock(clockId));
        }

        private ClockId CreateClock()
        {
            var clockId = new ClockId();

            CreateClock(clockId);

            return clockId;
        }

        private void CreateClock(ClockId clockId)
        {
            _commandProcessor.CommitCreation<ClockFactory>(f => f.Create(clockId));
        }

        private RigidBodyId FindRigidBodyId()
        {
            return _eventListener.FindEvents<RigidBodyCreated>().Single().RootId;
        }

        private void UpdateAll(TimeSpan elapsed)
        {
            _commandProcessor.CommitAll<Clock>(root => root.UpdateTime(elapsed));
            _commandProcessor.CommitAll<RigidBody>(root => root.UpdatePhysics());
        }

        private CreatureId CreateAvatar(WorldId worldId)
        {
            var creatureId = new CreatureId();

            CreateAvatar(creatureId, worldId);

            return creatureId;
        }

        private TerrainId CreateTerrain(Map<Tile> map)
        {
            var terrainId = new TerrainId();

            _commandProcessor.CommitCreation<TerrainFactory>(x => x.Create(terrainId, map));

            return terrainId;
        }

        private WorldId CreateWorld(ReadOnlyCollection<Vector> spawnPositions, ClockId clockId, TerrainId terrainId)
        {
            var worldId = new WorldId();

            CreateWorld(worldId, spawnPositions, clockId, terrainId);

            return worldId;
        }

        private void CreateWorld(WorldId worldId, ReadOnlyCollection<Vector> spawnPositions, ClockId clockId, TerrainId terrainId)
        {
            _commandProcessor.CommitCreation<WorldFactory>(f => f.Create(worldId, clockId, terrainId, spawnPositions, _playerProperties, spawnPositions, TimeSpan.FromSeconds(1), _heroesProperties));
        }

        private void CreateAvatar(CreatureId creatureId, WorldId worldId)
        {
            var world = _commandProcessor.CreateRootAdapter<World>(worldId);
            world.Commit(x => x.SpawnPlayerAvatar(creatureId));
        }

        private void MoveCreatureLeft(CreatureId creatureId)
        {
            _commandProcessor.Commit<Creature>(creatureId, creature => creature.SetMovement(Movement.Left));
        }

        private static ICommandProcessor<IDomainEvent> CreateBus(TestEventListener testEventListener)
        {
            var processor = new CommandProcessor<IDomainEvent>(testEventListener);

            processor.ConfigureDomain();

            return processor;
        }

        private class TestEventListener : IEventsListener<IDomainEvent>
        {
            private readonly List<IDomainEvent> _allEvents = new List<IDomainEvent>();

            public void AssertEvents(IEnumerable<IDomainEvent> expectedEvents)
            {
                CollectionAssert.AreEqual(expectedEvents.AsReadOnly(), _allEvents.AsReadOnly());
            }

            public void Recieve(IDomainEvent domainEvent)
            {
                _allEvents.Add(domainEvent);
            }

            public IEnumerable<TEvent> FindEvents<TEvent>()
            {
                return _allEvents.OfType<TEvent>();
            }
        }
    }
}
