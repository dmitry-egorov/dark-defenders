using System;

namespace DarkDefenders.Domain.Game.Interfaces
{
    public interface IGame
    {
        void Initialize(string mapId);
        void KillAllHeroes();
        void Update(TimeSpan elapsed);
        IPlayer AddPlayer();
        void SpawnHeros(int count);
        void ChangeSpawnHeroes(bool enabled);
    }
}