namespace DarkDefenders.Domain.Worlds.Events
{
    public interface IWorldEventsReciever
    {
        void Recieve(HeroSpawned heroSpawned);
        void Recieve(PlayerAvatarSpawned playerAvatarSpawned);
    }
}