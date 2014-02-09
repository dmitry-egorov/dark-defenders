using System;
using System.Collections.Generic;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.RigidBodies;
using DarkDefenders.Domain.Entities.Worlds;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Interfaces;
using DarkDefenders.Domain.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;
using Moq;
using NUnit.Framework;

namespace DarkDefenders.IntegrationTests
{
    [TestFixture]
    public class MainIntegrationTest
    {
        private const float Mass = 1.0f;
        private const float TopHorizontalMomentum = 40.0f;
        private const float BoundingBoxRadius = 0.4f;

        private static readonly Vector _spawnPosition = new Vector(50, 0.4f);

        private readonly WorldProperties _worldProperties;

        public MainIntegrationTest()
        {
            var heroesSpawnCooldown = TimeSpan.FromSeconds(10);
            var heroesRigidBodyProperties = new RigidBodyProperties(BoundingBoxRadius, Mass, TopHorizontalMomentum);
            var playersRigidBodyProperties = new RigidBodyProperties(BoundingBoxRadius, Mass, TopHorizontalMomentum);
            var heroesProperties = new CreatureProperties(180.0f, 60.0f, heroesRigidBodyProperties);
            var playersSpawnPositions = _spawnPosition.EnumerateOnce().AsReadOnly();
            var heroesSpawnPositions = _spawnPosition.EnumerateOnce().AsReadOnly();

            var playerProperties = new CreatureProperties(180.0f, 60.0f, playersRigidBodyProperties);
            _worldProperties = new WorldProperties(playersSpawnPositions, playerProperties, heroesSpawnPositions, heroesSpawnCooldown, heroesProperties);
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_creature_and_move_creature_on_update()
        {
            var dimensions = new Dimensions(100, 100);
            var mapId = "mapId";
            var map = new Map<Tile>(dimensions, default(Tile));
            map.Fill(Tile.Open);
            var elapsed = TimeSpan.FromMilliseconds(100);

            var newPosition = Vector.XY(48.2, 0.400000005960464);

            var reciever = new Mock<IEventsReciever>(MockBehavior.Strict);

            var expectedRigidBodyId = new IdentityOf<RigidBody>(4);
            var expectedCreatureId = new IdentityOf<Creature>(5);

            reciever.Setup(x => x.TerrainCreated(mapId));
            reciever.Setup(x => x.RigidBodyCreated(expectedRigidBodyId, _spawnPosition));
            reciever.Setup(x => x.CreatureCreated(expectedCreatureId, expectedRigidBodyId));
            reciever.Setup(x => x.PlayerAvatarSpawned(expectedCreatureId));
            reciever.Setup(x => x.Moved(expectedRigidBodyId, newPosition));

            var eventsProcessor = DelegatingEventsListener.Create(reciever.Object);
            var game = GameFactory.Create(eventsProcessor);

            var world = game.Initialize(mapId, map, _worldProperties);
            var avatar = world.AddPlayer();

            avatar.ChangeMovement(Movement.Left);

            game.Update(elapsed);

            reciever.VerifyAll();
        }
    }
}
