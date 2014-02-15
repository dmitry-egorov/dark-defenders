namespace Infrastructure.DDDES
{
    public interface IStorage<in TEntity>
    {
        void Store(TEntity item);
        void Remove(TEntity item);
    }
}