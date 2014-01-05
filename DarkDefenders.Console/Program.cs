using System;
using System.IO;
using System.Windows.Forms;
using DarkDefenders.Console.ViewModels;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Files;
using DarkDefenders.Domain.Other;
using DarkDefenders.Domain.Creatures;
using DarkDefenders.Domain.Projectiles;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations;
using Infrastructure.Math;
using Infrastructure.Math.Physics;
using Infrastructure.Util;

namespace DarkDefenders.Console
{
    static class Program
    {
        private static readonly RigidBodyProperties _playersRigidBodyProperties = new RigidBodyProperties(0.4, 1.0, 40.0);

        private const int MaxFps = 100;
        private const int TimeSlowdown = 1;
//        private const int TimeSlowdown = 5;
        private static readonly TimeSpan _minFrameElapsed = TimeSpan.FromSeconds(1.0 / MaxFps);
        private static readonly TimeSpan _playerStateUpdatePeriod = TimeSpan.FromSeconds(1.0 / 30);
        private static readonly TimeSpan _keyboardUpdatePeriod = TimeSpan.FromSeconds(1.0 / 100);

        private static readonly Vector _spawnPosition = new Vector(35, 65);
//        private const string WorldFileName = "simpleWorld3.txt";
//        private const string WorldFileName = "world1.bmp";
        private const string WorldFileName = "world2.bmp";

        private static readonly Toggle _limitFps = new Toggle(true);

        private static bool _escape;

        static void Main()
        {
            var renderer = new GameViewModel();

            var counter = new CountingEventsListener<IDomainEvent>();

            var composite = new CompositeEventsListener<IDomainEvent>(renderer, counter);

            var processor = CreateAndConfigureProcessor(composite, _playersRigidBodyProperties);

            var world = InitializeDomain(processor);
            var avatar = CreateAvatar(processor, world);
            var rigidBodies = processor.CreateRootsAdapter<RigidBody>();
            var worlds = processor.CreateRootsAdapter<World>();
            var projectiles = processor.CreateRootsAdapter<Projectile>();

            var keyBoardExecutor = new PeriodicExecutor(_keyboardUpdatePeriod);
            var fpsCounter = new PerformanceCounter();
            var eventsCounter = new PerformanceCounter();
            var creatureStateExecutor = new PeriodicExecutor(_playerStateUpdatePeriod);

            var clock = Clock.StartNew();
            var filler = TimeFiller.StartNew(_minFrameElapsed);

            while (!_escape)
            {
                var elapsed = clock.ElapsedSinceLastCall;
                var elapsedSeconds = (elapsed.TotalSeconds / TimeSlowdown).LimitTop(1.0).ToSeconds();

                keyBoardExecutor.Tick(elapsed, () => ProcessKeyboard(avatar, processor));

                worlds.DoAndCommit(x => x.UpdateWorldTime(elapsedSeconds));
                rigidBodies.DoAndCommit(x => x.UpdateMomentum());
                rigidBodies.DoAndCommit(x => x.UpdatePosition());
                projectiles.DoAndCommit(x => x.CheckForHit());

                creatureStateExecutor.Tick(elapsed, renderer.RenderCreatureState);
                fpsCounter.Tick(elapsed, renderer.RenderFps);
                eventsCounter.Tick(counter.EventsSinceLastCall, elapsed, renderer.RenderAverageEventsCount);

                _limitFps.WhenToggled(filler.FillTimeFrame);
            }
        }

        private static ICommandProcessor<IDomainEvent> CreateAndConfigureProcessor(IEventsListener<IDomainEvent> eventsListener, RigidBodyProperties playersRigidBodyProperties)
        {
            var processor = new CommandProcessor<IDomainEvent>(eventsListener);

            processor.ConfigureDomain(playersRigidBodyProperties);

            return processor;
        }

        private static IRootAdapter<World, IDomainEvent> InitializeDomain(ICommandProcessor<IDomainEvent> processor)
        {
            var worldId = new WorldId();
            var map = LoadTerrain();

            processor.CreateAndCommit<WorldFactory>(t => t.Create(worldId, map, _spawnPosition));

            return processor.CreateRootAdapter<World>(worldId);
        }

        private static IRootAdapter<Creature, IDomainEvent> CreateAvatar(ICommandProcessor<IDomainEvent> processor, IRootAdapter<World, IDomainEvent> world)
        {
            var creatureId = new CreatureId();
            world.DoAndCommit(x => x.SpawnPlayer(creatureId));

            return processor.CreateRootAdapter<Creature>(creatureId);
        }

        private static Map<Tile> LoadTerrain()
        {
            var pathToFiles = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WorldsData");
            var path = Path.Combine(pathToFiles, WorldFileName);

            return TerrainLoader.LoadFromFile(path);
        }

        private static void ProcessKeyboard(IRootAdapter<Creature, IDomainEvent> avatar, IUnitOfWork unitOfWork)
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

            unitOfWork.Commit();

            _limitFps.Tick(NativeKeyboard.IsKeyDown(Keys.L));

            if (NativeKeyboard.IsKeyDown(Keys.Escape))
            {
                _escape = true;
            }
        }
    }
}
