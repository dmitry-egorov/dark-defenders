namespace Infrastructure.DDDEventSourcing.Domain
{
    public interface IRepository<out TAggregateRoot, in TIdentity>
    {
        TAggregateRoot GetById(TIdentity id);
    }
}