namespace DarkDefenders.Remote.Serialization.Internals
{
    internal enum SerializableEvents: short
    {
        MapLoaded = 0,
        Created = 1,
        Destroyed = 2,
        Moved = 3,
        ChangedDirection = 4
    }
}