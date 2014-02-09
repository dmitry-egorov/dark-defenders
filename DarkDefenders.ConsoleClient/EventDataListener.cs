using System.Collections.Concurrent;
using System.Net.Sockets;
using DarkDefenders.Domain.Infrastructure;
using DarkDefenders.Domain.Serialization;
using Infrastructure.Util;

namespace DarkDefenders.ConsoleClient
{
    internal class EventDataListener
    {
        private readonly UdpClient _client;
        private volatile bool _stopped;
        private readonly ConcurrentQueue<byte[]> _queue = new ConcurrentQueue<byte[]>();
        private readonly EventsDeserializer _deserializer;

        public EventDataListener(IEventsReciever reciever)
        {
            _deserializer = new EventsDeserializer(reciever);
            _client = new UdpClient(1337);
        }

        public async void ListenAsync()
        {
            while (!_stopped)
            {
                var data = await _client.ReceiveAsync();
                _queue.Enqueue(data.Buffer);
            }
        }

        public void ProcessEvents()
        {
            var data = _queue.DequeueAllCurrent().AsReadOnly();

            foreach (var eventData in data)
            {
                _deserializer.Deserialize(eventData);
            }
        }

        public void Stop()
        {
            _stopped = true;
        }
    }
}