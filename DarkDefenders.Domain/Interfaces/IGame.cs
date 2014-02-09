using System;
using DarkDefenders.Domain.Entities.Worlds;
using DarkDefenders.Domain.Other;
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