using System;
using Infrastructure.Util;

namespace DarkDefenders.Server.Internals
{
    public class GameServerState
    {
        public TimeSpan LastActualElapsed { get; set; }

        public string GetText()
        {
            var elapsed = LastActualElapsed.TotalMilliseconds.ToInt();
            return "elapsed: " + elapsed + "ms";
        }
    }
}