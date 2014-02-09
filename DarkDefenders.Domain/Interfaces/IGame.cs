using System;
using DarkDefenders.Domain.Data.Entities.Worlds;
using DarkDefenders.Domain.Data.Other;
using Infrastructure.Math;

namespace DarkDefenders.Domain.Interfaces
{
    public interface IGame
    {
        IWorld Initialize(string mapId, Map<Tile> map, WorldProperties worldProperties);
        void KillAllHeroes();
        void Update(TimeSpan elapsed);
    }
}