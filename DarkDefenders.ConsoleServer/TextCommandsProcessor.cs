using System;
using DarkDefenders.Domain.Game.Interfaces;
using Infrastructure.Util;

namespace DarkDefenders.ConsoleServer
{
    internal class TextCommandsProcessor
    {
        private readonly IGame _game;
        private TimeSpan _lastActualElapsed;

        public TextCommandsProcessor(IGame game)
        {
            _game = game;
        }

        public Result ProcessTextCommand(string commandText, out Action action)
        {
            action = null;

            if (commandText.IsIn("q", "quit", "stop", "exit"))
            {
                return Result.StopRequested;
            }

            if (commandText == "stats")
            {
                Console.WriteLine("elapsed: " + _lastActualElapsed.TotalMilliseconds.ToInt() + "ms");
                return Result.CommandNotFount;
            }

            if (commandText == "kill")
            {
                action = _game.KillAllHeroes;
                return Result.CommandFound;
            }

            var commandTextParts = commandText.Split(' ');

            if (commandTextParts.Length == 2)
            {
                if (commandTextParts[0] == "spawn")
                {
                    var enable = commandTextParts[1] == "enable";
                    action = () => _game.ChangeSpawnHeroes(enable);
                    return Result.CommandFound;
                }

                if (commandTextParts[0] == "hero")
                {
                    int count;
                    if (!int.TryParse(commandTextParts[1], out count))
                    {
                        return Result.CommandNotFount;
                    }

                    action = () => _game.SpawnHeros(count);
                    return Result.CommandFound;
                }
            }

            return Result.CommandNotFount;
        }

        public void SetActualElapsed(TimeSpan actualElapsed)
        {
            _lastActualElapsed = actualElapsed;
        }

        public enum Result
        {
            CommandFound,
            CommandNotFount,
            StopRequested
        }
    }
}