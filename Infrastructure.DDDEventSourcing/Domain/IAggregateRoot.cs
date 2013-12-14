namespace Infrastructure.DDDEventSourcing.Domain
{
    public interface IAggregateRoot<out TState>
    {
        TState State { get; }
    }
}