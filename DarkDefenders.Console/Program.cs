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
using Infrastructure.Util;

namespace DarkDefenders.Console
{
    static class Program
    {
        private const int MaxFps = 100;
        private const int TimeSlowdown = 1;
//        private const int TimeSlowdown = 5;
        private static readonly TimeSpan _minFrameElapsed = TimeSpan.FromSeconds(1.0 / MaxFps);
        private static readonly TimeSpan _creatureStateUpdatePeriod = TimeSpan.FromSeconds(1.0 / 30);
        private static readonly TimeSpan _keyboardStateUpdatePeriod = TimeSpan.FromSeconds(1.0 / 100);

        private static readonly Vector _spawnPosition = new Vector(35, 5);
        private const string WorldFileName = "simpleWorld3.txt";
//        private const string WorldFileName = "world1.bmp";
//        private const string WorldFileName = "world2.bmp";

        private static readonly Toggle _limitFps = new Toggle(true);

        private static bool _escape;

        static void Main()
        {
            var renderer = new GameViewModel();

            var counter = new CountingEventsListener<IDomainEvent>();

            var composite = new CompositeEventsListener<IDomainEvent>(renderer, counter);

            var processor = CreateProcessor(composite);

            var avatar = InitializeDomain(processor);
            var rigidBodies = processor.CreateRootsAdapter<RigidBody>();
            var worlds = processor.CreateRootsAdapter<World>();
            var projectiles = processor.CreateRootsAdapter<Projectile>();

            var keyBoardExecutor = new PeriodicExecutor(_keyboardStateUpdatePeriod);
            var fpsCounter = new PerformanceCounter();
            var eventsCounter = new PerformanceCounter();
            var creatureStateExecutor = new PeriodicExecutor(_creatureStateUpdatePeriod);

            var clock = Clock.StartNew();
            var filler = TimeFiller.StartNew(_minFrameElapsed);

            while (!_escape)
            {
                var elapsed = clock.ElapsedSinceLastCall;
                var elapsedSeconds = (elapsed.TotalSeconds / TimeSlowdown).LimitTop(1.0);

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

        private static ICommandProcessor<IDomainEvent> CreateProcessor(IEventsListener<IDomainEvent> eventsListener)
        {
            var processor = new CommandProcessor<IDomainEvent>(eventsListener);

            processor.ConfigureDomain();

            return processor;
        }

        private static IRootAdapter<Creature, IDomainEvent> InitializeDomain(ICommandProcessor<IDomainEvent> processor)
        {
            var worldId = new WorldId();
            var creatureId = new CreatureId();
            var map = LoadTerrain();

            processor.CreateAndCommit<WorldFactory>(t => t.Create(worldId, map, _spawnPosition));
            processor.CreateAndCommit<CreatureFactory>(p => p.Create(creatureId, worldId));

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
