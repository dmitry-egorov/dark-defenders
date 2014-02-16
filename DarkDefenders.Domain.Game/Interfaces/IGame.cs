using System;

namespace DarkDefenders.Domain.Game.Interfaces
{
    public interface IGame
    {
        void Initialize(string mapId);
        void KillAllHeroes();
        void Update(TimeSpan elapsed);
        IPlayer AddPlayer();
        void SpawnHero();
        void ChangeSpawnHeroes(bool enabled);
    }
}