using System;
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
    static class Program
    {
        static void Main()
        {
            var renderer = new ConsoleRenderer(100, 40);

            renderer.Initialize();
            
            var bus = CreateBus(renderer);

            var spawnPosition = new Vector(0, 0);
            var terrainId = new TerrainId();

            bus.PublishTo<Terrain>(terrainId, t => t.Create(spawnPosition));
            var player  = bus.Create<Player>(new PlayerId(), p => p.Create(terrainId));

            var clock = Clock.StartNew();

            while (true)
            {
                if (NativeKeyboard.IsKeyDown(KeyCode.Left))
                {
                    player.Do(x => x.Move(MoveDirection.Left));
                }
                else if (NativeKeyboard.IsKeyDown(KeyCode.Right))
                {
                    player.Do(x => x.Move(MoveDirection.Right));
                }
                else
                {
                    player.Do(x => x.Stop());
                }

                var elapsed = clock.ElapsedSinceLastCall;

                bus.PublishToAllOfType<IUpdateable>(x => x.Update(elapsed));

                var fps = Math.Round(10000.0 / elapsed.TotalMilliseconds) / 10;

                renderer.RenderFps(fps);

                LimitFps(elapsed);
            }
        }

        private static IBus CreateBus(ConsoleRenderer renderer)
        {
            var processor = new CommandProcessor();

            processor.ConfigureDomain();

            var bus = new Bus(processor);

            bus.Subscribe(events => events.ForEach(renderer.Apply));

            return bus;
        }

        private static void LimitFps(TimeSpan elapsed)
        {
            const int maxFps = 200000;
            var minFrameElapsed = TimeSpan.FromMilliseconds(1000.0 / maxFps);

            if (elapsed < minFrameElapsed)
            {
                Thread.Sleep(minFrameElapsed - elapsed);
            }
        }
    }
}
