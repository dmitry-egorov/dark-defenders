using System;
using System.Collections.Generic;

namespace Infrastructure.DDDES.Implementations.Internal
{
    internal class AllRootsToProcessorAdapter<TRoot, TDomainEvent> : IAllRootsAdapter<TRoot, TDomainEvent> 
        where TDomainEvent : IEvent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRootCommandProcessor<TRoot, TDomainEvent> _rootCommandProcessor;

        public AllRootsToProcessorAdapter(IUnitOfWork unitOfWork, IRootCommandProcessor<TRoot, TDomainEvent> rootCommandProcessor)
        {
            _unitOfWork = unitOfWork;
            _rootCommandProcessor = rootCommandProcessor;
        }

        public void Do(Func<TRoot, IEnumerable<TDomainEvent>> command)
        {
            _rootCommandProcessor.ProcessAll(command);
        }

        public void DoAndCommit(Func<TRoot, IEnumerable<TDomainEvent>> command)
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