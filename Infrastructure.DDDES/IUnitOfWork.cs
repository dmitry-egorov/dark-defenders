namespace Infrastructure.DDDES
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
    }
}