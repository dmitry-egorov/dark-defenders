using Infrastructure.DDDES.Implementations.Domain.Exceptions;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class RootBase<TSnapshot, TId> : IRoot<TSnapshot>
        where TSnapshot: IRootSnapshot<TId>, new()
        where TId: Identity
    {
        public TSnapshot Snapshot { get; private set; }

        protected RootBase()
        {
            Snapshot = new TSnapshot();
        }

        private bool IsCreated()
        {
            return Snapshot.Id != null;
        }

        protected void AssertExists()
        {
            if (!IsCreated())
            {
                throw new RootDoesntExistException();
            }
        }

        protected void AssertDoesntExist()
        {
            if (IsCreated())
            {
                throw new RootAlreadyExistsException(Snapshot.Id);
            }
        }
    }
}