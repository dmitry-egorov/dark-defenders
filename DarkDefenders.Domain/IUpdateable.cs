using System;
using System.Collections.Generic;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain
{
    public interface IUpdateable
    {
        IEnumerable<IEvent> Update(TimeSpan elapsed);
    }
}