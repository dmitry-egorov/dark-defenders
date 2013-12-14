namespace Infrastructure.DDDES
{
    public interface IRoot<out TSnapshot>
    {
        TSnapshot Snapshot { get; }
    }
}