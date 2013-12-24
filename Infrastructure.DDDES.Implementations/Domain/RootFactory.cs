using Infrastructure.DDDES.Implementations.Domain.Exceptions;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class RootFactory<TRootId, TRoot, TCreationEvent> : IRootFactory<TRoot, TCreationEvent> 
        where TRootId: Identity
    {
        private readonly IRepository<TRootId, TRoot> _repository;

        protected RootFactory(IRepository<TRootId, TRoot> repository)
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

        TRoot IRootFactory<TRoot, TCreationEvent>.Handle(TCreationEvent creationEvent)
        {
            return Handle(creationEvent);
        }

        protected abstract TRoot Handle(TCreationEvent creationEvent);
    }
}