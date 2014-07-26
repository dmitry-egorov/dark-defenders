namespace Infrastructure.DDDES
{
    public interface IFactory<out TEntity>
    {
        TEntity Create();
    }
}