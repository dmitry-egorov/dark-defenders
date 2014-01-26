namespace DarkDefenders.Domain.Interfaces
{
    public interface IWorld
    {
        IPlayer AddPlayer();
        void SpawnHero();
        void ChangeSpawnHeroes(bool enabled);
    }
}