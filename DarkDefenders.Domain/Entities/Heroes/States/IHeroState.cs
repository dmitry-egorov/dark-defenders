using System.Collections.Generic;
using DarkDefenders.Domain.Data.Entities.Heroes;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Entities.Heroes.States
{
    internal interface IHeroState
    {
        IEnumerable<IEvent> Update();
        HeroStateData GetData();
    }
}