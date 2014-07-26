using System;
using System.Threading;
using DarkDefenders.Server;
using Infrastructure.Util;
using Microsoft.Practices.Unity;

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

        private const int MaxFps = 50;

        private static readonly TimeSpan _elapsedLimit = TimeSpan.FromSeconds(1);

        static void Main()
        {
            Console.Write("Loading...");

            var server = CreateGameServer();

            var source = new CancellationTokenSource();

            var task = server.RunAsync(source.Token, WorldFileName, MaxFps, _elapsedLimit);

            Console.WriteLine("done.");

            RunConsoleCommandsProcessing(server, source);

            task.Wait();
        }

        private static IGameServer CreateGameServer()
        {
            var container = new UnityContainer();

            container.RegisterGameServer();

            return container.Resolve<IGameServer>();
        }

        private static void RunConsoleCommandsProcessing(IGameServer server, CancellationTokenSource source)
        {
            while (true)
            {
                Console.Write("Input game command: ");
                
                var commandText = Console.ReadLine();
                if (commandText.IsIn("q", "quit", "stop", "exit"))
                {
                    source.Cancel();
                    return;
                }

                server.Command(commandText);
            }
        }
    }
}
