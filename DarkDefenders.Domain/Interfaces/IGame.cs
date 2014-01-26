using System;
using System.Collections.Generic;
using DarkDefenders.Dtos.Entities.Creatures;
using DarkDefenders.Dtos.Entities.Worlds;
using DarkDefenders.Dtos.Other;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Interfaces
{
    public interface IGame
    {
        IWorld Initialize(Map<Tile> map, WorldProperties worldProperties);
        void Update(TimeSpan elapsed);
        void KillAllHeroes();
    }
}