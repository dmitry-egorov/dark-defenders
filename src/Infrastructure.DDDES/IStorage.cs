namespace Infrastructure.DDDES
{
    public interface IStorage<in TEntity>
    {
        void Store(TEntity entity);
        void Remove(TEntity entity);
    }
}