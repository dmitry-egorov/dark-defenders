namespace Infrastructure.DDDEventSourcing.Domain
{
    public interface IRoot<out TState>
    {
        TState State { get; }
    }
}