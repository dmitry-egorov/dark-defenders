using Infrastructure.DDDEventSourcing.Domain;

namespace Infrastructure.DDDEventSourcing
{
    public interface ICommand
    {
        Identity AggregateRootId { get; }
    }

    public interface ICommand<out TAggregateRootIdentity>: ICommand
        where TAggregateRootIdentity: Identity
    {
        new TAggregateRootIdentity AggregateRootId { get; }
    }
}