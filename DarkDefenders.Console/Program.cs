using System;
using System.IO;
using System.Windows.Forms;
using DarkDefenders.Console.ViewModels;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Files;
using DarkDefenders.Domain.Interfaces;
using DarkDefenders.Dtos.Entities.Creatures;
using DarkDefenders.Dtos.Entities.RigidBodies;
using DarkDefenders.Dtos.Entities.Worlds;
using DarkDefenders.Dtos.Infrastructure;
using DarkDefenders.Dtos.Other;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations;
using Infrastructure.Util;
using Microsoft.Practices.Unity;

namespace DarkDefenders.Console
{
    static class Program
    {
        private const int MaxFps = 60;

//        private const string WorldFileName = "simpleWorld3.txt";
//        private const string WorldFileName = "world1.bmp";
//        private const string WorldFileName = "testHeroFalling.bmp";
//        private const string WorldFileName = "testHeroFalling2.bmp";
//        private const string WorldFileName = "testHeroFalling3.bmp";
//        private const string WorldFileName = "testHoleJump.bmp";
//        private const string WorldFileName = "testHoleJump2.bmp";
        private const string WorldFileName = "world3.bmp";

        private static readonly RigidBodyProperties _playersRigidBodyProperties = new RigidBodyProperties(0.4, 1.0, 40.0);
        private static readonly CreatureProperties _playersAvatarProperties = new CreatureProperties(180.0, 60.0, _playersRigidBodyProperties);
//        private static readonly RigidBodyProperties _playersRigidBodyProperties = new RigidBodyProperties(0.4, 1.0, 20.0);
//        private static readonly CreatureProperties _playersAvatarProperties = new CreatureProperties(180.0, 30.0, _playersRigidBodyProperties);
        
        private static readonly TimeSpan _heroesSpawnCooldown = TimeSpan.FromSeconds(10);
        private static readonly RigidBodyProperties _heroesRigidBodyProperties = new RigidBodyProperties(0.4, 1.0, 20.0);
        private static readonly CreatureProperties _heroesCreatureProperties = new CreatureProperties(180, 30, _heroesRigidBodyProperties);
        
        private static readonly TimeSpan _minFrameElapsed = TimeSpan.FromSeconds(1.0 / MaxFps);
        private static readonly TimeSpan _elapsedLimit = TimeSpan.FromSeconds(1);
        private static readonly TimeSpan _heroRenderingPeriod = TimeSpan.FromSeconds(1.0 / 100);
        private static readonly TimeSpan _statsRenderingPeriod = TimeSpan.FromSeconds(1.0 / 100);
        private static readonly TimeSpan _keyboardUpdatePeriod = TimeSpan.FromSeconds(1.0 / 100);
        private static readonly TimeSpan _testHeroSpawnPeriod = TimeSpan.FromSeconds(1.0 / 500);

        private static readonly OnOffSwitch _limitFpsSwitch = new OnOffSwitch(true);
        private static readonly OnOffSwitch _heroSpawnSwitch = new OnOffSwitch(true);

        private static readonly PeriodicExecutor _keyBoardExecutor = new PeriodicExecutor(_keyboardUpdatePeriod);
        private static readonly PeriodicExecutor _statsRenderingExecutor = new PeriodicExecutor(_statsRenderingPeriod);
        private static readonly SwitchablePeriodicExecutor _heroRenderingExecutor = new SwitchablePeriodicExecutor(_heroRenderingPeriod, true);
        private static readonly SwitchablePeriodicExecutor _testHeroSpawnExecutor = new SwitchablePeriodicExecutor(_testHeroSpawnPeriod, false);

        private static readonly PerformanceCounter _fpsCounter = new PerformanceCounter();
        private static readonly PerformanceCounter _eventsCounter = new PerformanceCounter();

        private static readonly OnOffSwitch _spawnHeroOnOffSwitch = new OnOffSwitch(true);
        private static readonly Button _killHeroesButton = new Button();

        private static readonly TimeFiller _timeFiller = new TimeFiller(_minFrameElapsed);

        private static bool _escape;

        static void Main()
        {
            var renderer = new GameViewModel();

            var counter = new CountingEventsListener<IEventDto>();

            var composite = new CompositeEventsListener<IEventDto>(renderer, counter);


            var game = CreateGame(composite);
            var world = InitializeGame(game);

            var player = world.AddPlayer();
            var stopwatch = AutoResetStopwatch.StartNew();
            _timeFiller.Start();

            while (!_escape)
            {
                var elapsed = stopwatch.ElapsedSinceLastCall.LimitTo(_elapsedLimit);

                game.Update(elapsed);

                _keyBoardExecutor.Tick(elapsed, () => ProcessKeyboard(game, player, world));
                _testHeroSpawnExecutor.Tick(elapsed, world.SpawnHero);

                _heroRenderingExecutor.Tick(elapsed, renderer.RenderCreatures);
                _statsRenderingExecutor.Tick(elapsed, renderer.RenderCreatureState);
                _fpsCounter.Tick(elapsed, renderer.RenderFps);
                _eventsCounter.Tick(counter.EventsSinceLastCall, elapsed, renderer.RenderAverageEventsCount);

                _limitFpsSwitch.WhenOn(_timeFiller.FillTimeFrame);
            }
        }

        private static IWorld InitializeGame(IGame game)
        {
            var terrainData = LoadTerrain();

            var worldProperties = new WorldProperties(terrainData.PlayerSpawns, _playersAvatarProperties, terrainData.HeroSpawns, _heroesSpawnCooldown, _heroesCreatureProperties);

            return game.Initialize(terrainData.Map, worldProperties);
        }

        private static IGame CreateGame(IEventsListener<IEventDto> eventsListener)
        {
            var container = new UnityContainer();
            container.RegisterInstance(eventsListener);
            container.RegisterDomain();

            return container.ResolveGame();
        }

        private static TerrainData LoadTerrain()
        {
            var pathToFiles = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WorldsData");
            var path = Path.Combine(pathToFiles, WorldFileName);

            return TerrainLoader.LoadFromFile(path);
        }

        private static void ProcessKeyboard(IGame game, IPlayer player, IWorld world)
        {
            var leftIsPressed = NativeKeyboard.IsKeyDown(Keys.Left);
            var rightIsPressed = NativeKeyboard.IsKeyDown(Keys.Right);
            if (leftIsPressed && !rightIsPressed)
            {
                player.ChangeMovement(Movement.Left);
            }
            else if (rightIsPressed && !leftIsPressed)
            {
                player.ChangeMovement(Movement.Right);
            }
            else
            {
                player.ChangeMovement(Movement.Stop);
            }

            if (NativeKeyboard.IsKeyDown(Keys.Up))
            {
                player.Jump();
            }

            if (NativeKeyboard.IsKeyDown(Keys.LControlKey) || NativeKeyboard.IsKeyDown(Keys.RControlKey))
            {
                player.Fire();
            }

            _spawnHeroOnOffSwitch.State(NativeKeyboard.IsKeyDown(Keys.H), world.ChangeSpawnHeroes);
            _killHeroesButton.State(NativeKeyboard.IsKeyDown(Keys.K), game.KillAllHeroes);

            _heroSpawnSwitch.State(NativeKeyboard.IsKeyDown(Keys.S));
            _limitFpsSwitch.State(NativeKeyboard.IsKeyDown(Keys.L));
            _heroRenderingExecutor.State(NativeKeyboard.IsKeyDown(Keys.R));
            _testHeroSpawnExecutor.State(NativeKeyboard.IsKeyDown(Keys.T));

            if (NativeKeyboard.IsKeyDown(Keys.Escape))
            {
                _escape = true;
            }
        }
    }
}
