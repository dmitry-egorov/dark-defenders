using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using DarkDefenders.Game.App;
using DarkDefenders.Game.App.Interfaces;
using DarkDefenders.Game.Resources;
using DarkDefenders.Remote.AdapterFromGame;
using DarkDefenders.Remote.Model;
using Infrastructure.DDDES;
using Infrastructure.Runtime;
using Infrastructure.Util;

namespace DarkDefenders.ConsoleServer
{
    class Program
    {
//        private const string WorldFileName = "simpleWorld3.txt";
//        private const string WorldFileName = "world1.bmp";
//        private const string WorldFileName = "testHeroFalling.bmp";
//        private const string WorldFileName = "testHeroFalling2.bmp";
//        private const string WorldFileName = "testHeroFalling3.bmp";
//        private const string WorldFileName = "testHoleJump.bmp";
//        private const string WorldFileName = "testHoleJump2.bmp";
        private const string WorldFileName = "world3.bmp";
        private const int MaxFps = 60;

        private static readonly TimeSpan _elapsedLimit = TimeSpan.FromSeconds(1);

        private static readonly ConcurrentQueue<Action> _commandsQueue = new ConcurrentQueue<Action>();

        static void Main()
        {
            Thread.Sleep(1000);//TODO: remove

            var game = InitializeGameService();

            var textCommandsProcessor = new TextCommandsProcessor(game);

            var loop = new Loop(MaxFps, _elapsedLimit);
            var gameTask = loop.RunParallel(elapsed =>
            {
                var actualElapsed = Measure.Time(() =>
                {
                    ExecuteCommands();

                    game.Update(elapsed);
                });

                textCommandsProcessor.SetActualElapsed(actualElapsed);
            });

            RunConsoleCommandsProcessing(textCommandsProcessor, loop);

            gameTask.Wait();
        }

        private static void ExecuteCommands()
        {
            var commands = _commandsQueue.DequeueAllCurrent().Take(100);

            foreach (var command in commands)
            {
                command();
            }
        }

        private static void RunConsoleCommandsProcessing(TextCommandsProcessor textCommandsProcessor, Loop loop)
        {
            while (true)
            {
                var commandText = Console.ReadLine();

                Action action;
                var result = textCommandsProcessor.ProcessTextCommand(commandText, out action);

                if (result == TextCommandsProcessor.Result.CommandFound)
                {
                    _commandsQueue.Enqueue(action);
                }
                else if (result == TextCommandsProcessor.Result.StopRequested)
                {
                    loop.Stop();
                    return;
                }
            }
        }

        private static IGameService InitializeGameService()
        {
            var networkBroadcaster = new EventsDataBroadcaster();

            var game = CreateGameService(networkBroadcaster);

            game.Initialize(WorldFileName);

            return game;
        }

        private static IGameService CreateGameService(IEventsListener<IRemoteEvents> networkBroadcaster)
        {
            return new GameBootstrapper()
            .RegisterResources()
            .RegisterRemoteEvents(networkBroadcaster)
            .Bootstrap();
        }
    }
}
