using System;
using System.Net;
using System.Threading;
using DarkDefenders.Game.App.Interfaces;

namespace DarkDefenders.Client
{
    public interface IGameClient
    {
        void RunAsync(CancellationToken cancellationToken, IPAddress ipAddress, int port);
        void Publish(Action<IPlayerService> command);
        void Pulse(TimeSpan elapsed);
    }
}