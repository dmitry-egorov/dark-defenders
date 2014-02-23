using System.Net;
using Infrastructure.Network.Subscription.Client.Interfaces;
using Infrastructure.Network.Subscription.Client.Internals;

namespace Infrastructure.Network.Subscription.Client
{
    public static class SubscriptionClient
    {
        public static ISubscriptionClient Create(IEventsDataInterpreter interpreter, IPAddress ipAddress, int port)
        {
            var connectionManager = new ClientConnectionsManager(interpreter);

            var client = Network.Client.Client.Create(ipAddress, port, connectionManager);

            return new Internals.Client(client, connectionManager);
        }
    }
}