using System.IO;

namespace Infrastructure.Network.Subscription.Server.Interfaces
{
    public interface IEventsDataSource
    {
        void WriteEventsData(BinaryWriter writer);
        void WriteInitialEventsData(BinaryWriter writer);
    }
}