using System;
using Infrastructure.Network.Interfaces;
using Infrastructure.Network.Subscription.Client.Interfaces;
using Infrastructure.Runtime;
using Infrastructure.Util;

namespace Infrastructure.Network.Subscription.Client.Internals
{
    internal class ClientConnectionsManager : IConnectionsManager, ICommandsDataSender
    {
        private readonly ActionProcessor _eventsProcessor;
        private readonly EventsDataReciever _eventsDataReciever;

        private IDataSender _sender;

        public ClientConnectionsManager(IEventsDataInterpreter interpreter)
        {
            _eventsProcessor = new ActionProcessor();
            _eventsDataReciever = new EventsDataReciever(interpreter, _eventsProcessor);
        }

        public IDataReciever OpenConnection(IDataSender sender)
        {
            _sender = sender;

            return _eventsDataReciever;
        }

        public void CloseConnection(IDataSender sender)
        {
            if (_sender != sender)
            {
                throw new InvalidOperationException("Wrong sender!");
            }

            _sender = null;
        }

        public bool TrySend(byte[] data)
        {
            try
            {
                return _sender.NullSafe(x => x.Send(data));
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }

        public void ProcessEvents()
        {
            _eventsProcessor.Process();
        }
    }
}