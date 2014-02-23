using System;
using DarkDefenders.Game.App.Interfaces;
using Infrastructure.Runtime;
using JetBrains.Annotations;

namespace DarkDefenders.Server.Internals
{
    [UsedImplicitly]
    internal class TextCommandsProcessor
    {
        private readonly GameServerState _gameGameServer;
        private readonly IGameService _gameService;
        private readonly ActionProcessor _processor = new ActionProcessor();

        public TextCommandsProcessor(GameServerState gameGameServer, IGameService gameService)
        {
            _gameGameServer = gameGameServer;
            _gameService = gameService;
        }

        public void Process()
        {
            _processor.Process();
        }

        public void Publish(string commandText)
        {
            if (commandText == "stats")
            {
                var text = _gameGameServer.GetText();
                Console.WriteLine(text);
                return;
            }

            if (commandText == "kill")
            {
                _processor.Publish(() => _gameService.KillAllHeroes());
                return;
            }

            var commandTextParts = commandText.Split(' ');

            if (commandTextParts.Length == 2)
            {
                if (commandTextParts[0] == "spawn")
                {
                    var enable = commandTextParts[1] == "enable";
                    _processor.Publish(() => _gameService.ChangeSpawnHeroes(enable));
                    return;
                }

                if (commandTextParts[0] == "hero")
                {
                    int count;
                    if (!int.TryParse(commandTextParts[1], out count))
                    {
                        return;
                    }

                    _processor.Publish(() => _gameService.SpawnHeros(count));
                    return;
                }
            }
        }
    }
}