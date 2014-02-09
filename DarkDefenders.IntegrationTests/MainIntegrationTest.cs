using System;
using System.Collections.Generic;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Data.Entities.Clocks;
using DarkDefenders.Domain.Data.Entities.Creatures;
using DarkDefenders.Domain.Data.Entities.RigidBodies;
using DarkDefenders.Domain.Data.Entities.Terrains;
using DarkDefenders.Domain.Data.Entities.Worlds;
using DarkDefenders.Domain.Data.Infrastructure;
using DarkDefenders.Domain.Data.Other;
using DarkDefenders.Domain.Entities.Clocks;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Entities.Terrains;
using DarkDefenders.Domain.Entities.Worlds;
using Infrastructure.Data;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Physics;
using Infrastructure.Util;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace DarkDefenders.IntegrationTests
{
    [TestFixture]
    public class MainIntegrationTest
    {
        private const float Mass = 1.0f;
        private const float TopHorizontalMomentum = 40.0f;
        private const float BoundingBoxRadius = 0.4f;

        private static readonly VectorData _spawnPosition = new VectorData(50, 0.4f);

        private readonly CreatureProperties _playerProperties;

        private readonly WorldProperties _worldProperties;
        private readonly RigidBodyInitialProperties _rigidBodyInitialProperties;

        public MainIntegrationTest()
        {
            var heroesSpawnCooldown = TimeSpan.FromSeconds(10);
            var heroesRigidBodyProperties = new RigidBodyProperties(BoundingBoxRadius, Mass, TopHorizontalMomentum);
            var playersRigidBodyProperties = new RigidBodyProperties(BoundingBoxRadius, Mass, TopHorizontalMomentum);
            var heroesProperties = new CreatureProperties(180.0f, 60.0f, heroesRigidBodyProperties);
            var playersSpawnPositions = _spawnPosition.EnumerateOnce().AsReadOnly();
            var heroesSpawnPositions = _spawnPosition.EnumerateOnce().AsReadOnly();
            var rigidBodyProperties = new RigidBodyProperties(BoundingBoxRadius, Mass, TopHorizontalMomentum);

            _playerProperties = new CreatureProperties(180.0f, 60.0f, playersRigidBodyProperties);
            _worldProperties = new WorldProperties(playersSpawnPositions, _playerProperties, heroesSpawnPositions, heroesSpawnCooldown, heroesProperties);
            _rigidBodyInitialProperties = new RigidBodyInitialProperties(Momentum.Zero.ToData(), _spawnPosition, rigidBodyProperties);
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_creature_and_move_creature_on_update()
        {
            var dimensions = new Dimensions(100, 100);
            var mapId = "mapId";
            var map = new Map<Tile>(dimensions, default(Tile));
            map.Fill(Tile.Open);
            var desiredOrientation = Movement.Left;
            var elapsed = TimeSpan.FromMilliseconds(20);

            var externalForce = Vector.XY(-180, 0).ToForce();
            var newMomentum = Vector.XY(-3.6, 0).ToMomentum();
            var newPosition = Vector.XY(49.928, 0.4);

            var eventsProcessor = new FakeEventsProcessor();
            var container = new UnityContainer();
            container.RegisterInstance<IEventsListener<EventDataBase>>(eventsProcessor);
            container.RegisterDomain();
            var game = container.ResolveGame();

            var world = game.Initialize(mapId, map, _worldProperties);
            var avatar = world.AddPlayer();

            avatar.ChangeMovement(Movement.Left);
            game.Update(elapsed);

            IdentityValueGenerator.Reset();

            var expectedClockId = new IdentityOf<Clock>();
            var expectedTerrainId = new IdentityOf<Terrain>();
            var expectedWorldId = new IdentityOf<World>();
            var expectedRigidBodyId = new IdentityOf<RigidBody>();
            var expectedCreatureId = new IdentityOf<Creature>();

            var expectedEvents = new EventDataBase[]
            {
                new ClockCreatedData(expectedClockId), 
                new TerrainCreatedData(expectedTerrainId, mapId), 
                new WorldCreatedData(expectedWorldId, _worldProperties),
                new RigidBodyCreatedData(expectedRigidBodyId, _rigidBodyInitialProperties),
                new CreatureCreatedData(expectedCreatureId, expectedRigidBodyId, _playerProperties),
                new PlayerAvatarSpawnedData(expectedWorldId, expectedCreatureId), 
                new MovementChangedData(expectedCreatureId, desiredOrientation),
                new ExternalForceChangedData(expectedRigidBodyId, externalForce),
                new TimeChangedData(expectedClockId, elapsed), 
                new AcceleratedAndMovedData(expectedRigidBodyId, newPosition.ToData(), newMomentum.ToData()),
            };

            eventsProcessor.Assert(expectedEvents);
        }

        private class FakeEventsProcessor : IEventsListener<EventDataBase>
        {
            private readonly List<EventDataBase> _events = new List<EventDataBase>();

            public void Recieve(EventDataBase @event)
            {
                _events.Add(@event);
            }

            public void Assert(EventDataBase[] expectedEvents)
            {
                var actualEvents = _events.ToArray();

                CollectionAssert.AreEqual(expectedEvents, actualEvents);
            }
        }
    }
}
