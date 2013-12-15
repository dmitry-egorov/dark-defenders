using System.Collections.Generic;
using Infrastructure.DDDES.Implementations.Domain.Exceptions;
using Infrastructure.Util;
using MoreLinq;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class RootBase<TId, TSnapshot, TRootEventReciever, TRootEvent> : IRoot<TRootEvent>
        where TRootEvent: IRootEvent<TId, TRootEventReciever> 
        where TSnapshot: class, TRootEventReciever, new()
        where TId: Identity
    {
        private TSnapshot _snapshot;
        public TId Id { get; private set; }

        public TSnapshot Snapshot
        {
            get
            {
                AssertExists();

                return _snapshot;
            }
        }

        public void Apply(IEnumerable<TRootEvent> events)
        {
            var readOnlyEvents = events.AsReadOnly();
            if (readOnlyEvents.Count == 0)
            {
                return;
            }

            if (!IsCreated())
            {
                _snapshot = new TSnapshot();
            }

            readOnlyEvents.ForEach(x => x.ApplyTo(_snapshot));
        }

        protected RootBase(TId id)
        {
            Id = id;
        }

        protected void AssertDoesntExist()
        {
            if (IsCreated())
            {
                throw new RootAlreadyExistsException(GetType().Name, Id);
            }
        }

        private void AssertExists()
        {
            if (!IsCreated())
            {
                throw new RootDoesntExistException(GetType().Name, Id);
            }
        }

        private bool IsCreated()
        {
            return _snapshot != null;
        }
    }
}