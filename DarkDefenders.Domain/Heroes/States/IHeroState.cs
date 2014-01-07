using System.Collections;
using System.Collections.Generic;
using DarkDefenders.Domain.Events;

namespace DarkDefenders.Domain.Heroes.States
{
    public interface IHeroState
    {
        IEnumerable<IDomainEvent> Update();
    }
}