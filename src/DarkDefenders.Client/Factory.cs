using DarkDefenders.Client.Internal;

namespace DarkDefenders.Client
{
    public static class GameClientFactory
    {
        public static IGameClient Create()
        {
            return new GameClient();
        }
    }
}