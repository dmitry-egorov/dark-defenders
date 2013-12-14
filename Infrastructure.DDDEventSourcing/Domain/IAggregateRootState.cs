namespace Infrastructure.DDDEventSourcing.Domain
{
    public interface IAggregateRootState<out TIdentity> : IState
    {
        TIdentity Id { get; }
    }
}