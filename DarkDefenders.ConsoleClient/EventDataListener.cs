using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading.Tasks;
using DarkDefenders.Remote.Model.Interface;
using DarkDefenders.Remote.Serialization;
using Infrastructure.DDDES;
using Infrastructure.Util;

namespace DarkDefenders.ConsoleClient
{
    internal class EventDataListener
    {
        private readonly IEventsListener<IRemoteEvents> _reciever;
        private readonly UdpClient _client;
        private readonly ConcurrentQueue<Action<IRemoteEvents>> _queue = new ConcurrentQueue<Action<IRemoteEvents>>();
        private readonly EventsDeserializer _deserializer;

        private volatile bool _stopped;

        public EventDataListener(IEventsListener<IRemoteEvents> reciever)
        {
            _reciever = reciever;
            _deserializer = new EventsDeserializer();
            _client = new UdpClient(1337);
        }

        public async void ListenAsync()
        {
            while (!_stopped)
            {
                var data = await _client.ReceiveAsync();
                Task.Run(() =>
                {
                    var acceptors = _deserializer.Deserialize(data.Buffer);

                    foreach (var acceptor in acceptors)
                    {
                        _queue.Enqueue(acceptor);
                    }
                });
            }
        }

        public void ProcessEvents()
        {
            var acceptors = _queue.DequeueAllCurrent().AsReadOnly();

            _reciever.Recieve(acceptors);
        }

        public void Stop()
        {
            _stopped = true;
        }
    }
}