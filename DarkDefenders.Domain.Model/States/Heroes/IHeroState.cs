using System.Collections.Generic;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.States.Heroes
{
    public interface IHeroState
    {
        IEnumerable<IEvent> Update();
    }
}