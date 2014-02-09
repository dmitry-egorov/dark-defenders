using System.Collections.Generic;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Heroes.States
{
    internal interface IHeroState
    {
        IEnumerable<IEvent> Update();
    }
}