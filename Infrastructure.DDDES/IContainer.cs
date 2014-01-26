namespace Infrastructure.DDDES
{
    public interface IContainer<out T>
    {
        T Item { get; }
    }
}