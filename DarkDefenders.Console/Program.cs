﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Players;
using DarkDefenders.Domain.Terrains;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations;
using Infrastructure.Math;
using Infrastructure.Util;
using MoreLinq;

namespace DarkDefenders.Console
{
    class Program
    {
        static void Main()
        {
            var eventStore = new EventStore();

            var bus = CreateBus(eventStore);

            var renderer = new ConsoleRenderer();

            bus.Subscribe(events => events.ForEach(renderer.Apply));

            var terrainId = new TerrainId();
            var playerId = new PlayerId();

            var spawnPosition = new Vector(0, 0);

            bus.PublishTo<Terrain>(terrainId, terrain => terrain.Create(terrainId, spawnPosition));
            bus.PublishTo<Player>(playerId, player => player.Create(playerId, terrainId));

            var sw = Stopwatch.StartNew();
            var last = TimeSpan.Zero;

            const int maxFps = 200000;
            var minFrameElapsed = TimeSpan.FromMilliseconds(1000.0 / maxFps);

            while (true)
            {
                if (NativeKeyboard.IsKeyDown(KeyCode.Left))
                {
                    bus.PublishTo<Player>(playerId, x => x.Move(MoveDirection.Left));
                }
                else if (NativeKeyboard.IsKeyDown(KeyCode.Right))
                {
                    bus.PublishTo<Player>(playerId, x => x.Move(MoveDirection.Right));
                }
                else
                {
                    bus.PublishTo<Player>(playerId, x => x.Stop());
                }

                var current = sw.Elapsed;
                var elapsed = current - last;
                last = current;

                bus.PublishToAllOfType<IUpdateable>(x => x.Update(elapsed));
                var fps = Math.Round(10000.0 / elapsed.TotalMilliseconds) / 10;
                renderer.RenderFps(fps);

                if (elapsed < minFrameElapsed)
                {
                    Thread.Sleep(minFrameElapsed - elapsed);
                }
            }
        }

        private static IBus CreateBus(IEventStore eventStore)
        {
            var processor = new CommandProcessor();

            processor.ConfigureDomain(eventStore);

            var bus = new Bus(processor);

            bus.Subscribe(eventStore.Append);

            return bus;
        }
    }
}
