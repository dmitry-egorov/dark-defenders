﻿using System;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Players;
using DarkDefenders.Domain.RigidBodies;
using DarkDefenders.Domain.Worlds;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations;
using Infrastructure.Math;

namespace DarkDefenders.Console
{
    static class Program
    {
        private const int MaxFps = 30;
        private static readonly TimeSpan _minFrameElapsed = TimeSpan.FromSeconds(1.0 / MaxFps);

        static void Main()
        {
            var renderer = ConsoleRenderer.InitializeNew();

            var countingListener = new CountingEventsListener();

            var composite = new CompositeEventsListener(renderer, countingListener);

            var processor = CreateProcessor(composite);

            var player = InitializeDomain(processor);
            var players = processor.Create<Player>();
            var rigidBodies = processor.Create<RigidBody>();

            var fpsCounter = new PerformanceCounter();
            var eventsCounter = new PerformanceCounter();
            var clock = Clock.StartNew();
            var executor = FixedTimeFrameExecutor.StartNew(_minFrameElapsed);

            while (true)
            {
                var elapsed = clock.ElapsedSinceLastCall;

                players.DoAndCommit(x => x.ApplyMovementForce());
                rigidBodies.DoAndCommit(x => x.UpdateKineticState(elapsed));

                fpsCounter.Tick(elapsed, renderer.RenderFps);
                eventsCounter.Tick(countingListener.EventsSinceLastCall, elapsed, renderer.RenderAverageEventsCount);
                renderer.RenderEventsCount(countingListener.TotalCount);

                executor.FillTimeFrame(() => ProcessKeyboard(player, processor));
            }
        }

        private static ICommandProcessor CreateProcessor(IEventsLinstener eventsLinstener)
        {
            var processor = new CommandProcessor(eventsLinstener);

            processor.ConfigureDomain();

            return processor;
        }

        private static RootToProcessorAdapter<Player> InitializeDomain(ICommandProcessor processor)
        {
            var worldId = new WorldId();
            var spawnPosition = new Vector(0, 0.03);

            processor.CreateAndCommit<World>(worldId, t => t.Create(spawnPosition));
            return processor.CreateAndCommit<Player>(new PlayerId(), p => p.Create(worldId));
        }

        private static void ProcessKeyboard(RootToProcessorAdapter<Player> player, IUnitOfWork unitOfWork)
        {
            var leftIsPressed  = NativeKeyboard.IsKeyDown(KeyCode.Left);
            var rightIsPressed = NativeKeyboard.IsKeyDown(KeyCode.Right);
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

            if (NativeKeyboard.IsKeyDown(KeyCode.Up))
            {
                player.Do(x => x.TryJump());
            }

            unitOfWork.Commit();
        }
    }
}
