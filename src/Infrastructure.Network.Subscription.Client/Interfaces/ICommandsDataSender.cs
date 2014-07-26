namespace Infrastructure.Network.Subscription.Client.Interfaces
{
    public interface ICommandsDataSender
    {
        bool TrySend(byte[] data);
    }
}