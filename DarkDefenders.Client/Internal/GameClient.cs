using System;
using System.Net;
using System.Threading;
using DarkDefenders.Client.Presenters;
using DarkDefenders.Game.App.Interfaces;
using DarkDefenders.Game.Serialization;
using DarkDefenders.Remote.Model;
using DarkDefenders.Remote.Serialization;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Network.Subscription.Client;
using Infrastructure.Network.Subscription.Client.Interfaces;
using Infrastructure.Runtime;

namespace DarkDefenders.Client.Internal
{
    internal class GameClient : IGameClient
    {
        private static readonly TimeSpan _heroRenderingPeriod = TimeSpan.FromSeconds(1.0 / 100);
        private static readonly TimeSpan _statsRenderingPeriod = TimeSpan.FromSeconds(1.0 / 100);
        private static readonly PeriodicExecutor _statsRenderingExecutor = new PeriodicExecutor(_statsRenderingPeriod);
        private static readonly SwitchablePeriodicExecutor _heroRenderingExecutor = new SwitchablePeriodicExecutor(_heroRenderingPeriod, true);

        private static readonly PerformanceCounter _fpsCounter = new PerformanceCounter();
        private static readonly PerformanceCounter _eventsCounter = new PerformanceCounter();

        private RemotePlayerService _remotePlayerService;
        private ISubscriptionClient _client;
        private GamePresenter _gamePresenter;
        private CountingEventsListener<IRemoteEvents> _counter;

        public void RunAsync(CancellationToken cancellationToken, IPAddress ipAddress, int port)
        {
            _gamePresenter = new GamePresenter();

            _counter = new CountingEventsListener<IRemoteEvents>();

            var rendererListener = DelegatingEventsListener.Create(_gamePresenter);

            var composite = CompositeEventsListener.Create(rendererListener, _counter);

            var interpreter = new RemoteEventsDataInterpreter(new EventsDeserializer(composite));

            _client = SubscriptionClient.Create(interpreter, ipAddress, port);

            var commandsDataSender = _client.RunAsync(cancellationToken);

            _remotePlayerService = new RemotePlayerService(commandsDataSender);
        }

        public void Pulse(TimeSpan elapsed)
        {
            _heroRenderingExecutor.Tick(elapsed, _gamePresenter.RenderCreatures);
            _statsRenderingExecutor.Tick(elapsed, _gamePresenter.RenderCreatureState);
            _fpsCounter.Tick(elapsed, _gamePresenter.RenderFps);
            _eventsCounter.Tick(_counter.EventsSinceLastCall, elapsed, _gamePresenter.RenderAverageEventsCount);

            _client.Pulse();
        }

        public void Publish(Action<IPlayerService> command)
        {
            command(_remotePlayerService);
        }
    }
}
