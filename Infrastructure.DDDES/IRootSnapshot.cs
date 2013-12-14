namespace Infrastructure.DDDES
{
    public interface IRootSnapshot<out TId>
    {
        TId Id { get; }
    }
}