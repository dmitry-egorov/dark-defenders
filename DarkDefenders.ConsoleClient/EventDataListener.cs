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
                var buffer = data.Buffer;

                Task.Run(() =>
                {
                    var actions = _deserializer.Deserialize(buffer);

                    foreach (var action in actions)
                    {
                        _queue.Enqueue(action);
                    }
                });
            }
        }

        public void ProcessEvents()
        {
            var actions = _queue.DequeueAllCurrent().AsReadOnly();

            _reciever.Recieve(actions);
        }

        public void Stop()
        {
            _stopped = true;
        }
    }
}