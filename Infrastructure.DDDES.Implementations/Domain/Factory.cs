using Infrastructure.DDDES.Implementations.Domain.Exceptions;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class Factory<TRootId, TRoot>
        where TRootId: Identity
    {
        private readonly IRepository<TRootId, TRoot> _repository;

        protected Factory(IRepository<TRootId, TRoot> repository)
        {
            _repository = repository;
        }

        protected void AssertDoesntExist(TRootId worldId)
        {
            if (_repository.Exists(worldId))
            {
                throw new RootAlreadyExistsException(GetType().Name, worldId);
            }
        }
    }
}