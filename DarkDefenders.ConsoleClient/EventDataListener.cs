using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using DarkDefenders.Domain.Data.Infrastructure;
using Infrastructure.DDDES;
using Infrastructure.Serialization;
using Infrastructure.Util;

namespace DarkDefenders.ConsoleClient
{
    internal class EventDataListener
    {
        private readonly UdpClient _client;
        private volatile bool _stopped;
        private readonly ConcurrentQueue<EventDataBase> _queue = new ConcurrentQueue<EventDataBase>();
        private readonly IEventsListener<EventDataBase> _reciever;

        public EventDataListener(IEventsListener<EventDataBase> reciever)
        {
            _reciever = reciever;
            _client = new UdpClient(1337);
        }

        public async Task ListenAsync()
        {
            while (!_stopped)
            {
                var data = await _client.ReceiveAsync();
                Task.Run(() =>
                {
                    var events = data.Buffer.ProtoDeserializeAs<List<EventDataBase>>();

                    foreach (var eventData in events)
                    {
                        _queue.Enqueue(eventData);
                    }
                });
            }
        }

        public void ProcessEvents()
        {
            var events = _queue.DequeueAllCurrent().AsReadOnly();

            foreach (var eventData in events)
            {
                _reciever.Recieve(eventData);
            }
        }

        public void Stop()
        {
            _stopped = true;
        }
    }
}