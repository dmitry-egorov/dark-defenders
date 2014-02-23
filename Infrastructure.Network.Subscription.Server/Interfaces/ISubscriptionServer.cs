using System.Threading;

namespace Infrastructure.Network.Subscription.Server.Interfaces
{
    public interface ISubscriptionServer
    {
        void Pulse();
        void RunAsync(CancellationToken cancellationToken);
    }
}