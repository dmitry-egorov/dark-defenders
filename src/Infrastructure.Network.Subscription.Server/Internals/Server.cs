using System.Threading;
using Infrastructure.Network.Subscription.Server.Interfaces;

namespace Infrastructure.Network.Subscription.Server.Internals
{
    internal class Server : ISubscriptionServer
    {
        private readonly Network.Server.Server _server;
        private readonly ServerConnectionManager _connectionManager;

        public Server(Network.Server.Server server, ServerConnectionManager connectionManager)
        {
            _server = server;
            _connectionManager = connectionManager;
        }

        public void RunAsync(CancellationToken cancellationToken)
        {
            _server.RunAsync(cancellationToken);
        }

        public void Pulse()
        {
            _connectionManager.ProcessClients();
        }
    }
}