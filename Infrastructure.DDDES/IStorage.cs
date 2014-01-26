namespace Infrastructure.DDDES
{
    public interface IStorage<in TRoot>
    {
        void Store(TRoot item);
        void Remove(TRoot item);
    }
}