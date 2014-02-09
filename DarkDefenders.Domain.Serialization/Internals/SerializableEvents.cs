namespace DarkDefenders.Domain.Serialization.Internals
{
    internal enum SerializableEvents: short
    {
        TerrainCreated = 0,
        RigidBodyCreated = 1,
        RigidBodyDestroyed = 2,
        Moved = 3,
        CreatureCreated = 4,
        HeroCreated = 5,
        HeroDestroyed = 6,
        PlayerAvatarSpawned = 7,
        ProjectileCreated = 8
    }
}