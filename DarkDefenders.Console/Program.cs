using System;
using System.IO;
using System.Windows.Forms;
using DarkDefenders.Console.ViewModels;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Events;
using DarkDefenders.Domain.Files;
using DarkDefenders.Domain.Players;
using DarkDefenders.Domain.Projectiles;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations;
using Infrastructure.Math;

namespace DarkDefenders.Console
{
    static class Program
    {
        private const int MaxFps = 300000000;
//        private const int MaxFps = 30;
        private static readonly TimeSpan _minFrameElapsed = TimeSpan.FromSeconds(1.0 / MaxFps);
        private static readonly TimeSpan _playerStateUpdatePeriod = TimeSpan.FromSeconds(1.0 / 30);
        private const string WorldFileName = "simpleWorld.txt";

        static void Main()
        {
            var renderer = new GameViewModel();

            var countingListener = new CountingEventsListener<IDomainEvent>();

            var composite = new CompositeEventsListener<IDomainEvent>(renderer, countingListener);

            var processor = CreateProcessor(composite);

            var player = InitializeDomain(processor);
            var players = processor.CreateRootsAdapter<Player>();
            var rigidBodies = processor.CreateRootsAdapter<RigidBody>();
            var worlds = processor.CreateRootsAdapter<World>();
            var projectiles = processor.CreateRootsAdapter<Projectile>();

            var fpsCounter = new PerformanceCounter();
            var eventsCounter = new PerformanceCounter();
            var playerStateCounter = new PerformanceCounter(_playerStateUpdatePeriod);

            var clock = Clock.StartNew();
            var executor = FixedTimeFrameExecutor.StartNew(_minFrameElapsed);

            var escape = false;
            while (!escape)
            {
                var elapsed = clock.ElapsedSinceLastCall;
                var elapsedSeconds = elapsed.TotalSeconds;

                worlds.DoAndCommit(x => x.UpdateWorldTime(elapsedSeconds));
                players.DoAndCommit(x => x.ApplyMovementForce());
                rigidBodies.DoAndCommit(x => x.UpdateMomentum());
                rigidBodies.DoAndCommit(x => x.UpdatePosition());
                projectiles.DoAndCommit(x => x.CheckForHit());

                var eventsCount = countingListener.EventsSinceLastCall;
                fpsCounter.Tick(elapsed, renderer.RenderFps);
                eventsCounter.Tick(eventsCount, elapsed, renderer.RenderAverageEventsCount);
                playerStateCounter.Tick(elapsed, (_, __) =>
                {
                    renderer.RenderPlayerState();
                    escape = ProcessKeyboard(player, processor);
                });

                executor.FillTimeFrame();
            }
        }

        private static ICommandProcessor<IDomainEvent> CreateProcessor(IEventsListener<IDomainEvent> eventsListener)
        {
            var processor = new CommandProcessor<IDomainEvent>(eventsListener);

            processor.ConfigureDomain();

            return processor;
        }

        private static IRootAdapter<Player, IDomainEvent> InitializeDomain(ICommandProcessor<IDomainEvent> processor)
        {
            var worldId = new WorldId();
            var spawnPosition = new Vector(50, 0.5);
            var playerId = new PlayerId();
            var map = LoadTerrain();

            processor.CreateAndCommit<WorldFactory>(t => t.Create(worldId, map, spawnPosition));
            processor.CreateAndCommit<PlayerFactory>(p => p.Create(playerId, worldId));

            return processor.CreateRootAdapter<Player>(playerId);
        }

        private static Map LoadTerrain()
        {
            var pathToFiles = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WorldsData");
            var path = Path.Combine(pathToFiles, WorldFileName);

            return TerrainLoader.LoadFromTextFile(path);
        }

        private static bool ProcessKeyboard(IRootAdapter<Player, IDomainEvent> player, IUnitOfWork unitOfWork)
        {
            var leftIsPressed = NativeKeyboard.IsKeyDown(Keys.Left);
            var rightIsPressed = NativeKeyboard.IsKeyDown(Keys.Right);
            if (leftIsPressed && !rightIsPressed)
            {
                player.Do(x => x.MoveLeft());
            }
            else if (rightIsPressed && !leftIsPressed)
            {
                player.Do(x => x.MoveRight());
            }
            else
            {
                player.Do(x => x.Stop());
            }

            if (NativeKeyboard.IsKeyDown(Keys.Up))
            {
                player.Do(x => x.Jump());
            }

            if (NativeKeyboard.IsKeyDown(Keys.LControlKey) || NativeKeyboard.IsKeyDown(Keys.RControlKey))
            {
                player.Do(x => x.Fire());
            }

            unitOfWork.Commit();

            return NativeKeyboard.IsKeyDown(Keys.Escape);
        }
    }
}
