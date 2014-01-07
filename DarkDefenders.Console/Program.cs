using System;
using System.IO;
using System.Windows.Forms;
using DarkDefenders.Console.ViewModels;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Clocks;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Files;
using DarkDefenders.Domain.Heroes;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Projectiles;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Terrains;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations;
using Infrastructure.Util;

namespace DarkDefenders.Console
{
    static class Program
    {
        private const int MaxFps = 100;

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

        private static readonly Button _spawnHeroButton = new Button();
        private static readonly Button _killHeroesButton = new Button();

        private static readonly TimeFiller _timeFiller = new TimeFiller(_minFrameElapsed);

        private static bool _escape;

        static void Main()
        {
            var renderer = new GameViewModel();

            var counter = new CountingEventsListener<IDomainEvent>();

            var composite = new CompositeEventsListener<IDomainEvent>(renderer, counter);

            var processor = CreateAndConfigureProcessor(composite);

            var clockId = new ClockId();
            var worldId = new WorldId();
            var avatarCreatureId = new CreatureId();

            InitializeDomain(processor, clockId, worldId, avatarCreatureId);

            var clock = processor.CreateRootAdapter<Clock>(clockId);
            var world = processor.CreateRootAdapter<World>(worldId);
            var avatar = processor.CreateRootAdapter<Creature>(avatarCreatureId);

            var rigidBodies = processor.CreateRootsAdapter<RigidBody>();
            var projectiles = processor.CreateRootsAdapter<Projectile>();
            var heroes = processor.CreateRootsAdapter<Hero>();

            var stopwatch = AutoResetStopwatch.StartNew();
            _timeFiller.Start();

            while (!_escape)
            {
                var elapsed = stopwatch.ElapsedSinceLastCall.LimitTo(_elapsedLimit);
                
                _keyBoardExecutor.Tick(elapsed, () => ProcessKeyboard(world, avatar, processor));

                clock.Commit(x => x.UpdateTime(elapsed));
                rigidBodies.Commit(x => x.UpdatePhysics());
                projectiles.Commit(x => x.CheckForHit());
                heroes.Commit(x => x.Think());
                _heroSpawnSwitch.WhenOn(() => world.Commit(x => x.SpawnHeroes()));
                _testHeroSpawnExecutor.Tick(elapsed, () => world.Commit(x => x.SpawnHero()));

                _heroRenderingExecutor.Tick(elapsed, renderer.RenderCreatures);
                _statsRenderingExecutor.Tick(elapsed, renderer.RenderCreatureState);
                _fpsCounter.Tick(elapsed, renderer.RenderFps);
                _eventsCounter.Tick(counter.EventsSinceLastCall, elapsed, renderer.RenderAverageEventsCount);

                _limitFpsSwitch.WhenOn(_timeFiller.FillTimeFrame);
            }
        }

        private static ICommandProcessor<IDomainEvent> CreateAndConfigureProcessor(IEventsListener<IDomainEvent> eventsListener)
        {
            var processor = new CommandProcessor<IDomainEvent>(eventsListener);

            processor.ConfigureDomain();

            return processor;
        }

        private static void InitializeDomain(ICommandProcessor<IDomainEvent> processor, ClockId clockId, WorldId worldId, CreatureId creatureId)
        {
            var terrainId = new TerrainId();
            var terrainData = LoadTerrain();

            processor.CommitCreation<ClockFactory>(t => t.Create(clockId));
            processor.CommitCreation<TerrainFactory>(t => t.Create(terrainId, terrainData.Map));
            processor.CommitCreation<WorldFactory>(t => t.Create(worldId, clockId, terrainId, terrainData.PlayerSpawns, _playersAvatarProperties, terrainData.HeroSpawns, _heroesSpawnCooldown, _heroesCreatureProperties));

            processor.Commit<World>(worldId, x => x.SpawnPlayerAvatar(creatureId));
        }

        private static TerrainData LoadTerrain()
        {
            var pathToFiles = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WorldsData");
            var path = Path.Combine(pathToFiles, WorldFileName);

            return TerrainLoader.LoadFromFile(path);
        }

        private static void ProcessKeyboard(IRootAdapter<World, IDomainEvent> world, IRootAdapter<Creature, IDomainEvent> avatar, IUnitOfWork unitOfWork)
        {
            var leftIsPressed = NativeKeyboard.IsKeyDown(Keys.Left);
            var rightIsPressed = NativeKeyboard.IsKeyDown(Keys.Right);
            if (leftIsPressed && !rightIsPressed)
            {
                avatar.Do(x => x.SetMovement(Movement.Left));
            }
            else if (rightIsPressed && !leftIsPressed)
            {
                avatar.Do(x => x.SetMovement(Movement.Right));
            }
            else
            {
                avatar.Do(x => x.SetMovement(Movement.Stop));
            }

            if (NativeKeyboard.IsKeyDown(Keys.Up))
            {
                avatar.Do(x => x.Jump());
            }

            if (NativeKeyboard.IsKeyDown(Keys.LControlKey) || NativeKeyboard.IsKeyDown(Keys.RControlKey))
            {
                avatar.Do(x => x.Fire());
            }

            _spawnHeroButton.State(NativeKeyboard.IsKeyDown(Keys.H), () => world.Do(x => x.SpawnHero()));
            _killHeroesButton.State(NativeKeyboard.IsKeyDown(Keys.K), () => world.Do(x => x.KillAllHeroes()));

            unitOfWork.Commit();

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
