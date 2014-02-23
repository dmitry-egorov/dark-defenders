using System;
using Infrastructure.Network.Subscription.Server.Interfaces;
using Infrastructure.Network.Subscription.Server.Internals;

namespace Infrastructure.Network.Subscription.Server
{
    public static class SubscriptionServer
    {
        public static ISubscriptionServer Create(IEventsDataWriter writer, Func<ICommandsDataInterpreter> interpretersFactory, int port)
        {
            var connectionManager = new ServerConnectionManager(writer, interpretersFactory);

            var server = Network.Server.Server.Create(port, connectionManager);

            return new Internals.Server(server, connectionManager);
        }
    }
}