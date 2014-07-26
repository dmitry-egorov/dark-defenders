using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Infrastructure.Network.Interfaces;
using Infrastructure.Network.Subscription.Server.Interfaces;
using Infrastructure.Runtime;
using Infrastructure.Serialization;
using Infrastructure.Util;

namespace Infrastructure.Network.Subscription.Server.Internals
{
    internal class ServerConnectionManager : IConnectionsManager
    {
        private readonly ConcurrentQueue<IDataSender> _connecting = new ConcurrentQueue<IDataSender>();
        private readonly ConcurrentQueue<IDataSender> _disconnecting = new ConcurrentQueue<IDataSender>();
        private readonly HashSet<IDataSender> _senders = new HashSet<IDataSender>();

        private readonly IEventsDataSource _eventsDataSource;
        private readonly Func<ICommandsDataInterpreter> _interpretersFactory;
        private readonly ActionProcessor _commandsProcessor = new ActionProcessor();

        public ServerConnectionManager(IEventsDataSource eventsDataSource, Func<ICommandsDataInterpreter> interpretersFactory)
        {
            _eventsDataSource = eventsDataSource;
            _interpretersFactory = interpretersFactory;
        }

        public IDataReciever OpenConnection(IDataSender sender)
        {
            _connecting.Enqueue(sender);

            var interpreter = _interpretersFactory();

            return new CommandsDataReciever(interpreter, _commandsProcessor);
        }

        public void CloseConnection(IDataSender sender)
        {
            _disconnecting.Enqueue(sender);
        }

        public void ProcessClients()
        {
            var disconnecting = GetDisconnecting();

            var connecting = GetConnecting(disconnecting);

            ProcessDisconnecting(disconnecting);

            ProcessConnecting(connecting);

            SendEvents();

            ProcessCommands();
        }

        private ReadOnlyCollection<IDataSender> GetConnecting(ICollection<IDataSender> disconnecting)
        {
            return _connecting
                .DequeueAllCurrent()
                .Where(x => !disconnecting.Contains(x))
                .AsReadOnly();
        }

        private ReadOnlyCollection<IDataSender> GetDisconnecting()
        {
            return _disconnecting.DequeueAllCurrent().AsReadOnly();
        }

        private void ProcessDisconnecting(IEnumerable<IDataSender> disconnecting)
        {
            foreach (var sender in disconnecting)
            {
                _senders.Remove(sender);
            }
        }

        private void ProcessConnecting(IReadOnlyCollection<IDataSender> connecting)
        {
            if (connecting.Count == 0)
            {
                return;
            }

            var currentState = GetInitialEventsData();

            foreach (var sender in connecting)
            {
                sender.Send(currentState);
                _senders.Add(sender);
            }
        }

        private void SendEvents()
        {
            var update = GetEventsData();

            foreach (var sender in _senders)
            {
                sender.Send(update);
            }
        }

        private void ProcessCommands()
        {
            _commandsProcessor.Process();
        }

        private byte[] GetEventsData()
        {
            return Using.GZipBinaryWriter(WriteEventsData);
        }

        private byte[] GetInitialEventsData()
        {
            return Using.GZipBinaryWriter(WriteInitialEventsData);
        }

        private void WriteEventsData(BinaryWriter writer)
        {
            writer.Write((byte) SubscriptionDataType.Update);
            _eventsDataSource.WriteEventsData(writer);
        }

        private void WriteInitialEventsData(BinaryWriter writer)
        {
            writer.Write((byte) SubscriptionDataType.InitialState);
            _eventsDataSource.WriteInitialEventsData(writer);
        }
    }
}