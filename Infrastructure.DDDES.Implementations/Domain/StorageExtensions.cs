namespace Infrastructure.DDDES.Implementations.Domain
{
    public static class StorageExtensions
    {
        public static IStorage<T> ComposeWith<T>(this IStorage<T> storage, IStorage<T> other)
        {
            return new CompositeStorage<T>(storage, other);
        }
    }
}