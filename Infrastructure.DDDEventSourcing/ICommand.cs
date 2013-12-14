using Infrastructure.DDDEventSourcing.Domain;

namespace Infrastructure.DDDEventSourcing
{
    public interface ICommand
    {
        Identity RootId { get; }
    }

    public interface ICommand<out TRootId>: ICommand
        where TRootId: Identity
    {
        new TRootId RootId { get; }
    }
}