namespace Infrastructure.DDDES
{
    public static class RepositoryExtensions
    {
        public static TRoot GetNew<TRootId, TRoot>(this IRepository<TRootId, TRoot> repository)
            where TRootId: new()
        {
            var id = new TRootId();
            return repository.GetById(id);
        }
    }
}