namespace Infrastructure.DDDES
{
    public interface IEntity<out TId>
    {
        TId GetGlobalId();
    }
}