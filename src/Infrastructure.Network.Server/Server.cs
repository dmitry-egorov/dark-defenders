using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Network.Interfaces;
using Infrastructure.Network.Internals;
using Infrastructure.Runtime.Collections;
using Infrastructure.Udp;

namespace Infrastructure.Network.Server
{
    public class Server
    {
        private const IOControlCode SioUdpConnreset = (IOControlCode) (-1744830452);

        private readonly ConcurrentDictionary<IPEndPoint, IDataReciever> _recieversMap = new ConcurrentDictionary<IPEndPoint, IDataReciever>();
        private readonly ConcurrentHashSet<IPEndPoint> _handshakingMap = new ConcurrentHashSet<IPEndPoint>();

        private readonly int _port;
        private readonly IConnectionsManager _connectionsManager;

        public static Server Create(int port, IConnectionsManager recieverFactoryFunc)
        {
            return new Server(port, recieverFactoryFunc);
        }

        private Server(int port, IConnectionsManager connectionsManager)
        {
            _port = port;
            _connectionsManager = connectionsManager;
        }

        public async void RunAsync(CancellationToken token)
        {
            await Task.Yield();

            var socket = CreateSocket();

            token.Register(socket.Close);

            try
            {
                while (!token.IsCancellationRequested)
                {
                    var result = await socket.ReceiveAsync();
                    var data = result.Buffer;
                    var endPoint = result.RemoteEndPoint;

                    IDataReciever reciever;
                    if (_recieversMap.TryGetValue(endPoint, out reciever))
                    {
                        reciever.Recieve(data);
                    }
                    else if (_handshakingMap.Contains(endPoint))
                    {
                        if (!TryConnectClientAsync(data, endPoint, socket))
                        {
                            Console.WriteLine("error. Wrong confirmation data!");
                        }
                        else
                        {
                            Console.WriteLine("done.");
                        }
                    }
                    else
                    {
                        Console.Write("Connecting client...");
                        if (!await TryHandshake(data, socket, endPoint))
                        {
                            Console.WriteLine("error. Wrong connect request data.");
                        }
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private async Task<bool> TryHandshake(byte[] data, UdpClient socket, IPEndPoint endPoint)
        {
            if (!ConnectionData.VerifyConnectData(data))
            {
                return false;
            }
            
            var handshakeData = ConnectionData.GetHandshakeData();

            await socket.SendAsync(handshakeData, endPoint);

            _handshakingMap.TryAdd(endPoint);

            return true;
        }

        private UdpClient CreateSocket()
        {
            var endPoint = new IPEndPoint(IPAddress.Any, _port);
            var client = new UdpClient(endPoint);
            client.Client.IOControl(SioUdpConnreset, new byte[] {0, 0, 0, 0}, null);
            return client;
        }

        private bool TryConnectClientAsync(byte[] data, IPEndPoint endPoint, UdpClient client)
        {
            if (!ConnectionData.VerifyConfirmData(data))
            {
                _handshakingMap.TryRemove(endPoint);
                return false;
            }

            var sender = new DataSender(client, endPoint);

            var reciever = _connectionsManager.OpenConnection(sender);

            if (!_recieversMap.TryAdd(endPoint, reciever))
            {
                _connectionsManager.CloseConnection(sender);
                return false;
            }

            return true;
        }
    }
}