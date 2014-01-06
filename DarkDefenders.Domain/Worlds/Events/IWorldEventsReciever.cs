namespace DarkDefenders.Domain.Worlds.Events
{
    public interface IWorldEventsReciever
    {
        void Recieve(HeroesSpawned heroesSpawned);
        void Recieve(PlayerAvatarSpawned playerAvatarSpawned);
    }
}