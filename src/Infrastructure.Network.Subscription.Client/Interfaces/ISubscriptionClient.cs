using System.Threading;

namespace Infrastructure.Network.Subscription.Client.Interfaces
{
    public interface ISubscriptionClient
    {
        ICommandsDataSender RunAsync(CancellationToken cancellationToken);
        void Pulse();
    }
}