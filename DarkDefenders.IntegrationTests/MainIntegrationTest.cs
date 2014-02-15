using System;
using DarkDefenders.Domain.Model;
using DarkDefenders.Domain.Model.Entities.Creatures;
using DarkDefenders.Domain.Model.Entities.RigidBodies;
using DarkDefenders.Domain.Model.Entities.Worlds;
using DarkDefenders.Domain.Model.Other;
using DarkDefenders.Game;
using DarkDefenders.Game.Interfaces;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Math;
using Infrastructure.Util;
using NSubstitute;
using NUnit.Framework;

namespace DarkDefenders.IntegrationTests
{
    [TestFixture]
    public class MainIntegrationTest
    {
        private const string ResourceId = "mapId";
        private static readonly Vector _spawnPosition = new Vector(50, 0.4f);

        private readonly IEventsReciever _reciever;
        private readonly IResources<Map<Tile>> _mapResources;
        private readonly IResources<WorldProperties> _propertiesResources;

        public MainIntegrationTest()
        {
            var playersSpawnPositions = _spawnPosition.EnumerateOnce().AsReadOnly();
            var heroesSpawnPositions = _spawnPosition.EnumerateOnce().AsReadOnly();

            var worldProperties = new WorldProperties(playersSpawnPositions, heroesSpawnPositions);

            var dimensions = new Dimensions(100, 100);
            var map = new Map<Tile>(dimensions, default(Tile));
            map.Fill(Tile.Open);

            _reciever = Substitute.For<IEventsReciever>();

            _mapResources = Substitute.For<IResources<Map<Tile>>>();
            _mapResources[ResourceId].Returns(map);

            _propertiesResources = Substitute.For<IResources<WorldProperties>>();
            _propertiesResources[ResourceId].Returns(worldProperties);
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_creature_and_move_creature_on_update()
        {
            var game = Setup();

            Act(game);

            Verify();
        }

        private IGame Setup()
        {
            var eventsProcessor = DelegatingEventsListener.Create(_reciever);
            return GameFactory.Create(eventsProcessor, _mapResources, _propertiesResources);
        }

        private void Act(IGame game)
        {
            var elapsed = TimeSpan.FromMilliseconds(100);

            game.Initialize(ResourceId);
            var avatar = game.AddPlayer();

            avatar.ChangeMovement(Movement.Left);

            game.Update(elapsed);
            game.Update(elapsed);
        }

        private void Verify()
        {
            var newPosition = Vector.XY(48.2, 0.40000000596046448);

            var expectedRigidBodyId = new IdentityOf<RigidBody>(7);
            var expectedCreatureId = new IdentityOf<Creature>(9);

            _reciever.Received().TerrainCreated(ResourceId);
            _reciever.Received().RigidBodyCreated(expectedRigidBodyId, _spawnPosition);
            _reciever.Received().CreatureCreated(expectedCreatureId, expectedRigidBodyId);
            _reciever.Received().PlayerCreated(expectedCreatureId);
            _reciever.Received().Moved(expectedRigidBodyId, newPosition);
        }
    }
}
