using System;
using System.Threading;
using System.Threading.Tasks;
using DarkDefenders.Game.App.Interfaces;
using Infrastructure.Network.Subscription.Server;
using Infrastructure.Runtime;

namespace DarkDefenders.Server.Internals
{
    internal class GameServer : IGameServer
    {
        private readonly IGameService _gameService;
        private readonly RemoteEventsDataSource _source;
        private readonly TextCommandsProcessor _processor;
        private readonly CommandInterpretersManager _interpretersManager;
        private readonly GameServerState _state;

        public GameServer
        (
            IGameService gameService, 
            RemoteEventsDataSource source, 
            TextCommandsProcessor processor, 
            CommandInterpretersManager interpretersManager,
            GameServerState state
        )
        {
            _gameService = gameService;
            _source = source;
            _processor = processor;
            _interpretersManager = interpretersManager;
            _state = state;
        }

        public Task RunAsync(CancellationToken cancellationToken, string mapId, int maxFps, TimeSpan elapsedLimit)
        {
            var server = SubscriptionServer.Create(_source, () => _interpretersManager.Create(), 1337);

            _gameService.Initialize(mapId);

            server.RunAsync(cancellationToken);

            var loop = new Loop(maxFps, elapsedLimit);

            return loop.RunParallel(elapsed =>
            {
                _state.LastActualElapsed = Measure.Time(() =>
                {
                    server.Pulse();
                    _processor.Process();
                    _gameService.Update(elapsed);
                });
            }, 
            cancellationToken);
        }

        public void Command(string command)
        {
            _processor.Publish(command);
        }
    }
}
