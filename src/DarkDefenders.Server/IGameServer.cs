using System;
using System.Threading;
using System.Threading.Tasks;

namespace DarkDefenders.Server
{
    public interface IGameServer
    {
        Task RunAsync(CancellationToken cancellationToken, string mapId, int maxFps, TimeSpan elapsedLimit);
        void Command(string command);
    }
}