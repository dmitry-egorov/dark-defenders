namespace Infrastructure.DDDES
{
    public interface IEvent<out TRootId>: IEvent
    {
        new TRootId RootId { get; }
    }

    public interface IEvent
    {
        Identity RootId { get; }
    }
}