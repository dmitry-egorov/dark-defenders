﻿using System;
using System.Collections.Generic;
using System.Linq;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Creatures.Events;
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
        public void Should_create_creature()
        {
            var spawnPosition = new Vector(0, 0);
            var dimensions = new Dimensions(10, 10);
            var map = new Map<Tile>(dimensions, default(Tile));

            var boundingBox = CreateCreaturesBoundingBox(spawnPosition);
            var mass = Creature.Mass;
            var topHorizontalMomentum = Creature.TopHorizontalMomentum;

            var worldId = CreateWorld(map, spawnPosition);
            var avatarId = CreateAvatar(worldId);
            var rigidBodyId = FindRigidBodyId();

            var expectedEvents = new IDomainEvent[]
            {
                new WorldCreated(worldId, map, spawnPosition), 
                new RigidBodyCreated(rigidBodyId, worldId, boundingBox, Vector.Zero, mass, topHorizontalMomentum),
                new CreatureCreated(avatarId, worldId, rigidBodyId)
            };

            _eventListener.AssertEvents(expectedEvents);
        }

        private static Box CreateCreaturesBoundingBox(Vector spawnPosition)
        {
            return new Box(spawnPosition, Creature.BoundingBoxRadius, Creature.BoundingBoxRadius);
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_creature()
        {
            var spawnPosition = new Vector(0, 0);
            var dimensions = new Dimensions(10, 10);
            var map = new Map<Tile>(dimensions, default(Tile));
            var boundingBox = CreateCreaturesBoundingBox(spawnPosition);
            var desiredOrientation = Movement.Left;
            var mass = Creature.Mass;
            var topHorizontalMomentum = Creature.TopHorizontalMomentum;

            var worldId = CreateWorld(map, spawnPosition);
            var creatureId = CreateAvatar(worldId);
            var rigidBodyId = FindRigidBodyId();

            MoveCreatureLeft(creatureId);

            var expectedEvents = new IDomainEvent[]
            {
                new WorldCreated(worldId, map, spawnPosition), 
                new RigidBodyCreated(rigidBodyId, worldId, boundingBox, Vector.Zero, mass, topHorizontalMomentum),
                new CreatureCreated(creatureId, worldId, rigidBodyId),
                new MovementChanged(creatureId, desiredOrientation), 
            };
            _eventListener.AssertEvents(expectedEvents);
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_creature_and_move_creature_on_update()
        {
            var spawnPosition = new Vector(50, 0.5);
            var dimensions = new Dimensions(100, 100);
            var map = new Map<Tile>(dimensions, default(Tile));
            var boundingBox = CreateCreaturesBoundingBox(spawnPosition);
            var desiredOrientation = Movement.Left;
            var elapsed = TimeSpan.FromMilliseconds(20).TotalSeconds;

            var externalForce = Vector.XY(-200, 0);
            var newMomentum = Vector.XY(-4, 0);
            var newPosition = Vector.XY(49.92, 0.5);
            var mass = Creature.Mass;
            var topHorizontalMomentum = Creature.TopHorizontalMomentum;

            var worldId = CreateWorld(map, spawnPosition);
            var creatureId = CreateAvatar(worldId);
            var rigidBodyId = FindRigidBodyId();

            MoveCreatureLeft(creatureId);
            UpdateAll(elapsed);

            var expectedEvents = new IDomainEvent[]
            {
                new WorldCreated(worldId, map, spawnPosition), 
                new RigidBodyCreated(rigidBodyId, worldId, boundingBox, Vector.Zero, mass, topHorizontalMomentum),
                new CreatureCreated(creatureId, worldId, rigidBodyId),
                new MovementChanged(creatureId, desiredOrientation), 
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
            var creatureId = new CreatureId();

            Assert.Throws<RootDoesntExistException>(() => CreateAvatar(creatureId, fakeWorldId));
        }

        [Test]
        public void Should_throw_when_world_is_created_twice()
        {
            var spawnPosition = new Vector(0, 0);
            var worldId = new WorldId();
            var dimensions = new Dimensions(10, 10);
            var map = new Map<Tile>(dimensions, default(Tile));

            CreateWorld(worldId, map, spawnPosition);

            Assert.Throws<RootAlreadyExistsException>(() => CreateWorld(worldId, map, spawnPosition));
        }

        private RigidBodyId FindRigidBodyId()
        {
            return _eventListener.FindEvents<RigidBodyCreated>().Single().RootId;
        }

        private void UpdateAll(double elapsed)
        {
            _commandProcessor.ProcessAllAndCommit<World>(root => root.UpdateWorldTime(elapsed));
            _commandProcessor.ProcessAllAndCommit<RigidBody>(root => root.UpdateMomentum());
            _commandProcessor.ProcessAllAndCommit<RigidBody>(root => root.UpdatePosition());
        }

        private CreatureId CreateAvatar(WorldId worldId)
        {
            var creatureId = new CreatureId();

            CreateAvatar(creatureId, worldId);

            return creatureId;
        }

        private WorldId CreateWorld(Map<Tile> map, Vector spawnPosition)
        {
            var worldId = new WorldId();

            CreateWorld(worldId, map, spawnPosition);

            return worldId;
        }

        private void CreateWorld(WorldId worldId, Map<Tile> map, Vector spawnPosition)
        {
            _commandProcessor.CreateAndCommit<WorldFactory>(f => f.Create(worldId, map, spawnPosition));
        }

        private void CreateAvatar(CreatureId creatureId, WorldId worldId)
        {
            _commandProcessor.CreateAndCommit<CreatureFactory>(f => f.Create(creatureId, worldId));
        }

        private void MoveCreatureLeft(CreatureId creatureId)
        {
            _commandProcessor.ProcessAndCommit<Creature>(creatureId, creature => creature.SetMovement(Movement.Left));
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
