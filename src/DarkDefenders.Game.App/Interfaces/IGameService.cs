using System;

namespace DarkDefenders.Game.App.Interfaces
{
    public interface IGameService
    {
        void Initialize(string mapId);
        void KillAllHeroes();
        void Update(TimeSpan elapsed);
        IPlayerService AddPlayer();
        void SpawnHeros(int count);
        void ChangeSpawnHeroes(bool enabled);
    }
}