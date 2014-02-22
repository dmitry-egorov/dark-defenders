using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Runtime.Collections;
using Infrastructure.Util;

namespace Infrastructure.Network.Server
{
    public class BroadcastingServer
    {
        private const IOControlCode SioUdpConnreset = (IOControlCode) (-1744830452);

        private readonly ConcurrentHashSet<IPEndPoint> _handshakeEndPoints = new ConcurrentHashSet<IPEndPoint>();
        private readonly ConcurrentDictionary<IPEndPoint, IDataReciever> _broadcastEndPoints = new ConcurrentDictionary<IPEndPoint, IDataReciever>();
        private readonly UdpClient _client;
        private readonly Func<IDataReciever> _recieverFactoryFunc;

        public static BroadcastingServer Create(int port, Func<IDataReciever> recieverFactoryFunc)
        {
            var endPoint = new IPEndPoint(IPAddress.Any, port);
            var udpClient = new UdpClient(endPoint);
            udpClient.Client.IOControl(SioUdpConnreset, new byte[] { 0, 0, 0, 0 }, null);

            return new BroadcastingServer(udpClient, recieverFactoryFunc);
        }

        private BroadcastingServer(UdpClient client, Func<IDataReciever> recieverFactoryFunc)
        {
            _client = client;
            _recieverFactoryFunc = recieverFactoryFunc;
        }

        public async void RecieveConnectionsAsync(CancellationToken token)
        {
            await Task.Yield();

            while (!token.IsCancellationRequested)
            {
                var result = await _client.ReceiveAsync();

                var endpoint = result.RemoteEndPoint;

                IDataReciever reciever;
                var connected = _broadcastEndPoints.TryGetValue(endpoint, out reciever);
                if (connected)
                {
                    reciever.Recieve(result.Buffer);
                    continue;
                }

                if (!_handshakeEndPoints.TryAdd(endpoint))
                {
                    //TODO: Signal of some problem (shouldn't recieve anything while handshaking)
                    Debug.WriteLine("Data recieved while handshaking!");
                }
            }
        }

        public async void BroadcastAsync(Func<byte[]> broadcastDataFunc, Func<byte[]> handshakeDataFunc)
        {
            await Task.Yield();

            await Handshake(handshakeDataFunc);

            await BroadcastConnected(broadcastDataFunc);
        }

        private async Task Handshake(Func<byte[]> handshakeDataFunc)
        {
            var endPoints = _handshakeEndPoints.AsReadOnly();

            if (endPoints.Count != 0)
            {
                var data = handshakeDataFunc();

                await SendAllAsync(data, endPoints);

                foreach (var endPoint in endPoints)
                {
                    if (!_handshakeEndPoints.TryRemove(endPoint))
                    {
                        Debug.WriteLine("Unable to remove endpoint from handshake!");
                        //TODO: warn
                    }

                    var reciever = _recieverFactoryFunc();

                    if (!_broadcastEndPoints.TryAdd(endPoint, reciever))
                    {
                        Debug.WriteLine("Unable to add endpoint to broadcast!");
                        //TODO: warn
                    }
                }
            }
        }

        private async Task BroadcastConnected(Func<byte[]> broadcastDataFunc)
        {
            var data = broadcastDataFunc();
            var broadcastEndPoints = _broadcastEndPoints.Keys;

            await SendAllAsync(data, broadcastEndPoints);
        }

        private async Task SendAllAsync(byte[] data, IEnumerable<IPEndPoint> ipEndPoints)
        {
            var bytes = data.Length;

            if (bytes == 0)
            {
                return;
            }

            var tasks = SendAll(data, bytes, ipEndPoints);

            await Task.WhenAll(tasks);
        }

        private IEnumerable<Task<int>> SendAll(byte[] data, int bytes, IEnumerable<IPEndPoint> ipEndPoints)
        {
            foreach (var endPoint in ipEndPoints)
            {
                yield return _client.SendAsync(data, bytes, endPoint);
            }
        }
    }
}