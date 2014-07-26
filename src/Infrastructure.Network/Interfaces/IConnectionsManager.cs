namespace Infrastructure.Network.Interfaces
{
    public interface IConnectionsManager
    {
        IDataReciever OpenConnection(IDataSender sender);
        void CloseConnection(IDataSender sender);
    }
}