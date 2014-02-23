namespace Infrastructure.Network.Interfaces
{
    public interface IDataSender
    {
        void Send(byte[] data);
    }
}