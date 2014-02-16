using System;
using DarkDefenders.Domain.Game.Interfaces;
using Infrastructure.Util;

namespace DarkDefenders.ConsoleServer
{
    internal class TextCommandsProcessor
    {
        private readonly IGame _game;

        public TextCommandsProcessor(IGame game)
        {
            _game = game;
        }

        public Result ProcessTextCommand(string commandText, out Action action)
        {
            if (commandText.IsIn("q", "quit", "stop", "exit"))
            {
                action = null;
                return Result.StopRequested;
            }

            if (commandText == "kill")
            {
                action = _game.KillAllHeroes;
                return Result.CommandFound;
            }

            if (commandText == "hero")
            {
                action = _game.SpawnHero;
                return Result.CommandFound;
            }

            var commandTextParts = commandText.Split(' ');

            if (commandTextParts.Length == 2 && commandTextParts[0] == "spawn")
            {
                var enable = commandTextParts[1] == "enable";
                action = () => _game.ChangeSpawnHeroes(enable);
                return Result.CommandFound;
            }

            action = null;
            return Result.CommandNotFount;
        }

        public enum Result
        {
            CommandFound,
            CommandNotFount,
            StopRequested
        }
    }
}