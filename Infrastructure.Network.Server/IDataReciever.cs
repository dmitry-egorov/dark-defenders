namespace Infrastructure.Network.Server
{
    public interface IDataReciever
    {
        void Recieve(byte[] buffer);
    }
}