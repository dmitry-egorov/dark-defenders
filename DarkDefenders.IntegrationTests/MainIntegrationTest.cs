using System;
using System.Collections.ObjectModel;
using DarkDefenders.Game.App;
using DarkDefenders.Game.App.Interfaces;
using DarkDefenders.Game.Model.Entities;
using DarkDefenders.Game.Model.EntityProperties;
using DarkDefenders.Game.Model.Events;
using DarkDefenders.Game.Model.Other;
using DarkDefenders.Game.Resources;
using DarkDefenders.Kernel.Model;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Physics;
using Infrastructure.Util;
using Microsoft.Practices.Unity;
using Moq;
using NSubstitute;
using NUnit.Framework;

namespace DarkDefenders.IntegrationTests
{
    [TestFixture]
    public class MainIntegrationTest
    {
        private const string ResourceId = "mapId";
        private static readonly Vector _spawnPosition = new Vector(50, 0.4f);

        private readonly Mock<IClockEvents> _clock = new Mock<IClockEvents>(MockBehavior.Strict);
        private readonly Mock<ITerrainEvents> _terrain = new Mock<ITerrainEvents>(MockBehavior.Strict);
        private readonly Mock<IPlayerSpawnerEvents> _playerSpawner = new Mock<IPlayerSpawnerEvents>(MockBehavior.Strict);
        private readonly Mock<IHeroSpawnerEvents> _heroSpawner = new Mock<IHeroSpawnerEvents>(MockBehavior.Strict);
        private readonly Mock<IHeroSpawnPointEvents> _heroSpawnPoint = new Mock<IHeroSpawnPointEvents>(MockBehavior.Strict);
        private readonly Mock<IWorldEvents> _world = new Mock<IWorldEvents>(MockBehavior.Strict);
        private readonly Mock<IRigidBodyEvents> _rigidBody = new Mock<IRigidBodyEvents>(MockBehavior.Strict);
        private readonly Mock<IWeaponEvents> _weapon = new Mock<IWeaponEvents>(MockBehavior.Strict);
        private readonly Mock<ICreatureEvents> _creature = new Mock<ICreatureEvents>(MockBehavior.Strict);
        private readonly Mock<IPlayerEvents> _player = new Mock<IPlayerEvents>(MockBehavior.Strict);

        private readonly IResources<Map<Tile>> _mapResources = Substitute.For<IResources<Map<Tile>>>();
        private readonly IResources<WorldProperties> _propertiesResources = Substitute.For<IResources<WorldProperties>>();
        private readonly ReadOnlyCollection<Vector> _heroesSpawnPositions = _spawnPosition.EnumerateOnce().AsReadOnly();
        private readonly ReadOnlyCollection<Vector> _playersSpawnPositions = _spawnPosition.EnumerateOnce().AsReadOnly();
        private readonly TimeSpan _elapsed = TimeSpan.FromMilliseconds(100);

        public MainIntegrationTest()
        {
            var worldProperties = new WorldProperties(_playersSpawnPositions, _heroesSpawnPositions);

            var dimensions = new Dimensions(100, 100);
            var map = new Map<Tile>(dimensions, default(Tile));
            map.Fill(Tile.Open);

            _mapResources[ResourceId].Returns(map);
            _propertiesResources[ResourceId].Returns(worldProperties);
        }

        [Test]
        public void Should_create_and_set_desired_orientation_to_creature_and_move_creature_on_update()
        {
            var game = Setup();

            Expect();

            Act(game);
        }

        private IGameService Setup()
        {
            return new UnityContainer()
            .ToGameBootstrapper()

            .RegisterResource(_mapResources)
            .RegisterResource(_propertiesResources)
            .RegisterHardcodedResources()

            .RegisterListener(() => _clock.Object)
            .RegisterListener(() => _terrain.Object)
            .RegisterListener(() => _playerSpawner.Object)
            .RegisterListener(() => _heroSpawner.Object)
            .RegisterListener(() => _heroSpawnPoint.Object)
            .RegisterListener(() => _world.Object)
            .RegisterListener(() => _rigidBody.Object)
            .RegisterListener(() => _weapon.Object)
            .RegisterListener(() => _creature.Object)
            .RegisterListener(() => _player.Object)

            .Bootstrap();
        }

        private void Act(IGameService game)
        {
            game.Initialize(ResourceId);
            var avatar = game.AddPlayer();

            avatar.ChangeMovement(Movement.Left);

            game.Update(_elapsed);
            game.Update(_elapsed);
        }

        private void Expect()
        {
            _clock         .Setup(x => x.Created());
            _terrain       .Setup(x => x.Created(ResourceId));
            _playerSpawner .Setup(x => x.Created(ResourceId));
            _heroSpawner   .Setup(x => x.Created(It.IsAny<ReadOnlyCollection<HeroSpawnPoint>>()));
            _heroSpawnPoint.Setup(x => x.Created(_spawnPosition));
            _world         .Setup(x => x.Created());
            _rigidBody     .Setup(x => x.Created(It.IsAny<RigidBody>(), _spawnPosition, Momentum.Zero, "Player"));
            _weapon        .Setup(x => x.Created(It.IsAny<RigidBody>()));
            _creature      .Setup(x => x.Created(It.IsAny<Creature>(), It.IsAny<RigidBody>(), "Player"));
            _player        .Setup(x => x.Created(It.IsAny<Creature>()));
            _creature      .Setup(x => x.MovementChanged(Movement.Left, Direction.Left));
            _rigidBody     .Setup(x => x.ExternalForceChanged(new Force(-180, 0)));
            _clock         .Setup(x => x.TimeChanged(_elapsed));
            _rigidBody     .Setup(x => x.Accelerated(new Momentum(-18, 0)));
            _rigidBody     .Setup(x => x.Moved(Vector.XY(48.2, 0.40000000596046448)));
            _clock         .Setup(x => x.TimeChanged(_elapsed + _elapsed));
        }
    }
}
