using System.Threading;
using Infrastructure.Network.Subscription.Client.Interfaces;

namespace Infrastructure.Network.Subscription.Client.Internals
{
    internal class Client : ISubscriptionClient
    {
        private readonly Network.Client.Client _client;
        private readonly ClientConnectionsManager _connectionsManager;

        public Client(Network.Client.Client client, ClientConnectionsManager connectionsManager)
        {
            _client = client;
            _connectionsManager = connectionsManager;
        }

        public ICommandsDataSender RunAsync(CancellationToken cancellationToken)
        {
            _client.RunAsync(cancellationToken);

            return _connectionsManager;
        }

        public void Pulse()
        {
            _connectionsManager.ProcessEvents();
        }
    }
}