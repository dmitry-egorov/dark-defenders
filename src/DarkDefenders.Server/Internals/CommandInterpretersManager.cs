using DarkDefenders.Game.App.Interfaces;
using Infrastructure.Network.Subscription.Server.Interfaces;
using JetBrains.Annotations;

namespace DarkDefenders.Server.Internals
{
    [UsedImplicitly]
    internal class CommandInterpretersManager
    {
        private readonly IGameService _gameService;

        public CommandInterpretersManager(IGameService gameService)
        {
            _gameService = gameService;
        }

        public ICommandsDataInterpreter Create()
        {
            var player = _gameService.AddPlayer();

            return new GameCommandsDataInterpreter(player);
        }
    }
}