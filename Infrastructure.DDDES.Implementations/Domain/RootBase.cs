using System.Collections.Generic;
using Infrastructure.DDDES.Implementations.Domain.Exceptions;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations.Domain
{
    public abstract class RootBase<TId, TRootEventReciever, TRootEvent> : IRoot<TRootEvent>
        where TRootEvent: IRootEvent<TId, TRootEventReciever> 
        where TId: Identity
    {
        public TId Id { get; private set; }

        private bool _isCreated;

        public void Apply(IEnumerable<TRootEvent> events)
        {
            var readOnlyEvents = events.AsReadOnly();
            if (readOnlyEvents.Count == 0)
            {
                return;
            }

            _isCreated = true;

            readOnlyEvents.ForEach(x => x.ApplyTo((TRootEventReciever)(object)this));
        }

        protected RootBase(TId id)
        {
            Id = id;
        }

        protected void AssertDoesntExist()
        {
            if (_isCreated)
            {
                throw new RootAlreadyExistsException(GetType().Name, Id);
            }
        }

        protected void AssertExists()
        {
            if (!_isCreated)
            {
                throw new RootDoesntExistException(GetType().Name, Id);
            }
        }
    }
}