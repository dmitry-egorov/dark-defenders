namespace Infrastructure.DDDEventSourcing.Domain
{
    public interface IRepository<out TRoot, in TRootId>
    {
        TRoot GetById(TRootId id);
    }
}