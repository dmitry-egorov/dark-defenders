using System;
using System.Collections.Generic;
using DarkDefenders.Remote.Model;

namespace DarkDefenders.Remote.AdapterFromGame
{
    public interface IRemoteEventsSource
    {
        IEnumerable<Action<IRemoteEvents>> GetEvents();
        IEnumerable<Action<IRemoteEvents>> GetCurrentStateEvents();
    }
}