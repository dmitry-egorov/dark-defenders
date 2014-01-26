using System;
using System.Collections.Generic;
using DarkDefenders.Domain;
using DarkDefenders.Dtos.Entities.Clocks;
using DarkDefenders.Dtos.Entities.Creatures;
using DarkDefenders.Dtos.Entities.RigidBodies;
using DarkDefenders.Dtos.Entities.Terrains;
using DarkDefenders.Dtos.Entities.Worlds;
using DarkDefenders.Dtos.Infrastructure;
using DarkDefenders.Dtos.Other;
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
        private const double Mass = 1.0;
        private const double TopHorizontalMomentum = 40.0;
        private const double BoundingBoxRadius = 0.4;

        private static readonly Vector _spawnPosition = new Vector(50, 0.4);

        private readonly CreatureProperties _playerProperties;

        private readonly WorldProperties _worldProperties;
        private readonly RigidBodyInitialProperties _rigidBodyInitialProperties;

        public MainIntegrationTest()
        {
            var heroesSpawnCooldown = TimeSpan.FromSeconds(10);
            var heroesRigidBodyProperties = new RigidBodyProperties(BoundingBoxRadius, Mass, TopHorizontalMomentum);
            var playersRigidBodyProperties = new RigidBodyProperties(BoundingBoxRadius, Mass, TopHorizontalMomentum);
            var heroesProperties = new CreatureProperties(180.0, 60.0, heroesRigidBodyProperties);
            var playersSpawnPositions = _spawnPosition.EnumerateOnce().AsReadOnly();
            var heroesSpawnPositions = _spawnPosition.EnumerateOnce().AsReadOnly();
            var rigidBodyProperties = new RigidBodyProperties(BoundingBoxRadius, Mass, TopHorizontalMomentum);

            _playerProperties = new CreatureProperties(180.0, 60.0, playersRigidBodyProperties);
            _worldProperties = new WorldProperties(playersSpawnPositions, _playerProperties, heroesSpawnPositions, heroesSpawnCooldown, heroesProperties);
            _rigidBodyInitialProperties = new RigidBodyInitialProperties(Momentum.Zero, _spawnPosition, rigidBodyProperties);
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_creature_and_move_creature_on_update()
        {
            var dimensions = new Dimensions(100, 100);
            var map = new Map<Tile>(dimensions, default(Tile));
            map.Fill(Tile.Open);
            var desiredOrientation = Movement.Left;
            var elapsed = TimeSpan.FromMilliseconds(20);

            var externalForce = Vector.XY(-180, 0).ToForce();
            var newMomentum = Vector.XY(-3.6, 0).ToMomentum();
            var newPosition = Vector.XY(49.928, 0.4);

            var eventsProcessor = new FakeEventsProcessor();
            var container = new UnityContainer();
            container.RegisterInstance<IEventsListener<IEventDto>>(eventsProcessor);
            container.RegisterDomain();
            var game = container.ResolveGame();

            var world = game.Initialize(map, _worldProperties);
            var avatar = world.AddPlayer();

            avatar.ChangeMovement(Movement.Left);
            game.Update(elapsed);

            Identity.Reset();

            var expectedClockId = new ClockId();
            var expectedTerrainId = new TerrainId();
            var expectedWorldId = new WorldId();
            var expectedRigidBodyId = new RigidBodyId();
            var expectedCreatureId = new CreatureId();

            var expectedEvents = new IEventDto[]
            {
                new ClockCreatedDto(expectedClockId), 
                new TerrainCreatedDto(expectedTerrainId, map), 
                new WorldCreatedDto(expectedWorldId, _worldProperties),
                new RigidBodyCreatedDto(expectedRigidBodyId, _rigidBodyInitialProperties),
                new CreatureCreatedDto(expectedCreatureId, expectedRigidBodyId, _playerProperties),
                new PlayerAvatarSpawnedDto(expectedWorldId, expectedCreatureId), 
                new MovementChangedDto(expectedCreatureId, desiredOrientation),
                new ExternalForceChangedDto(expectedRigidBodyId, externalForce),
                new TimeChangedDto(expectedClockId, elapsed), 
                new AcceleratedAndMovedDto(expectedRigidBodyId, newPosition, newMomentum),
            };

            eventsProcessor.Assert(expectedEvents);
        }

        private class FakeEventsProcessor : IEventsListener<IEventDto>
        {
            private readonly List<IEventDto> _events = new List<IEventDto>();

            public void Recieve(IEventDto @event)
            {
                _events.Add(@event);
            }

            public void Assert(IEventDto[] expectedEvents)
            {
                var actualEvents = _events.ToArray();

                CollectionAssert.AreEqual(expectedEvents, actualEvents);
            }
        }
    }
}
