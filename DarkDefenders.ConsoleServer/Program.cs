using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using DarkDefenders.Domain;
using DarkDefenders.Domain.Data.Entities.Creatures;
using DarkDefenders.Domain.Data.Entities.RigidBodies;
using DarkDefenders.Domain.Data.Entities.Worlds;
using DarkDefenders.Domain.Files;
using DarkDefenders.Domain.Interfaces;
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

        private static readonly RigidBodyProperties _playersRigidBodyProperties = new RigidBodyProperties(0.4f, 1.0f, 40.0f);
        private static readonly CreatureProperties _playersAvatarProperties = new CreatureProperties(180.0f, 60.0f, _playersRigidBodyProperties);
//        private static readonly RigidBodyProperties _playersRigidBodyProperties = new RigidBodyProperties(0.4, 1.0, 20.0);
//        private static readonly CreatureProperties _playersAvatarProperties = new CreatureProperties(180.0, 30.0, _playersRigidBodyProperties);

        private static readonly TimeSpan _heroesSpawnCooldown = TimeSpan.FromSeconds(10);
        private static readonly RigidBodyProperties _heroesRigidBodyProperties = new RigidBodyProperties(0.4f, 1.0f, 20.0f);
        private static readonly CreatureProperties _heroesCreatureProperties = new CreatureProperties(180, 30, _heroesRigidBodyProperties);
        private static readonly TimeSpan _elapsedLimit = TimeSpan.FromSeconds(1);

        private static readonly ConcurrentQueue<Action> _commandsQueue = new ConcurrentQueue<Action>();

        static void Main()
        {
            var networkBroadcaster = new EventsDataBroadcaster(10000);

            var game = GameFactory.Create(networkBroadcaster);

            var world = InitializeGame(game);

            var textCommandsProcessor = new TextCommandsProcessor(game, world);

            var stopwatch = new AutoResetStopwatch();

            var loopRunner = new LoopRunner(60, () => Frame(stopwatch, game, networkBroadcaster));

            var gameTask = Task.Factory.StartNew(() => Run(stopwatch, loopRunner), TaskCreationOptions.LongRunning);

            RunConsoleCommandsProcessing(textCommandsProcessor, loopRunner);

            gameTask.Wait();
        }

        private static void Run(AutoResetStopwatch stopwatch, LoopRunner loopRunner)
        {
            stopwatch.Start();
            loopRunner.Run();
        }

        private static void Frame(AutoResetStopwatch stopwatch, IGame game, EventsDataBroadcaster eventsDataBroadcaster)
        {
            var elapsed = stopwatch.ElapsedSinceLastCall.LimitTo(_elapsedLimit);

            game.Update(elapsed);

            ExecuteCommands();

            eventsDataBroadcaster.Broadcast();
        }

        private static void ExecuteCommands()
        {
            var commands = _commandsQueue.DequeueAllCurrent().Take(100);

            foreach (var command in commands)
            {
                command();
            }
        }

        private static void RunConsoleCommandsProcessing(TextCommandsProcessor textCommandsProcessor, LoopRunner loopRunner)
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
                    loopRunner.Stop();
                    return;
                }
            }
        }

        private static IWorld InitializeGame(IGame game)
        {
            var terrainData = LoadTerrain();

            var worldProperties = new WorldProperties(terrainData.PlayerSpawns, _playersAvatarProperties, terrainData.HeroSpawns, _heroesSpawnCooldown, _heroesCreatureProperties);

            return game.Initialize(WorldFileName, terrainData.Map.ToMap(), worldProperties);
        }

        private static TerrainData LoadTerrain()
        {
            return TerrainLoader.LoadFromFile(WorldFileName);
        }
    }
}
