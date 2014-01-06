using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations.Internal
{
    internal class RootToProcessorAdapter<TRoot, TDomainEvent> : IRootAdapter<TRoot, TDomainEvent> 
        where TDomainEvent : IEvent
    {
        private readonly IRootCommandProcessor<TRoot, TDomainEvent> _commandProcessor;
        private readonly Identity _id;
        private readonly IUnitOfWork _unitOfWork;

        public RootToProcessorAdapter(Identity id, IRootCommandProcessor<TRoot, TDomainEvent> commandProcessor, IUnitOfWork unitOfWork)
        {
            _commandProcessor = commandProcessor;
            _unitOfWork = unitOfWork;
            _id = id;
        }

        public void Do(Func<TRoot, IEnumerable<TDomainEvent>> command)
        {
            _commandProcessor.Process(_id, command);
        }

        public void Commit(Func<TRoot, IEnumerable<TDomainEvent>> command)
        {
            try
            {
                Do(command);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
