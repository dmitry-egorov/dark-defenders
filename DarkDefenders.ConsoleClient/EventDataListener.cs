using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using DarkDefenders.Domain.Interfaces;
using DarkDefenders.Domain.Serialization;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace DarkDefenders.ConsoleClient
{
    internal class EventDataListener
    {
        private readonly IEventsListener<IEventsReciever> _reciever;
        private readonly UdpClient _client;
        private volatile bool _stopped;
        private readonly ConcurrentQueue<IAcceptorOf<IEventsReciever>> _queue = new ConcurrentQueue<IAcceptorOf<IEventsReciever>>();
        private readonly EventsDeserializer _deserializer;

        public EventDataListener(IEventsListener<IEventsReciever> reciever)
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
                    var acceptors = _deserializer
                                  .Deserialize(data.Buffer)
                                  .Select(action => new ActionAcceptorOf<IEventsReciever>(action));

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