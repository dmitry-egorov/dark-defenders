using System;
using System.Collections.Generic;
using Infrastructure.DDDEventSourcing.Domain;
using Infrastructure.Util;

namespace Infrastructure.DDDEventSourcing.Implementations
{
    public class CommandProcessor : ICommandProcessor<ICommand>
    {
        private readonly Dictionary<Type, Func<ICommand, IEnumerable<IEventMarker>>> _handleActions = new Dictionary<Type, Func<ICommand, IEnumerable<IEventMarker>>>();

        public void AddHandlerFor<TCommand, TRoot, TRootId>(IRepository<TRoot, TRootId> repository)
            where TCommand : ICommand<TRootId>
            where TRoot : ICommandProcessor<TCommand>
            where TRootId : Identity
        {
            _handleActions[typeof(TCommand)] = command => Process(repository, (TCommand)command);
        }

        private static IEnumerable<IEventMarker> Process<TCommand, TRoot, TRootId>(IRepository<TRoot, TRootId> repository, TCommand command)
            where TCommand : ICommand<TRootId>
            where TRoot : ICommandProcessor<TCommand>
            where TRootId : Identity
        {
            var root = repository.GetById(command.RootId);

            return root.Process(command).AsReadOnly();
        }

        public IEnumerable<IEventMarker> Process(ICommand command)
        {
            Func<ICommand, IEnumerable<IEventMarker>> action;
            if (!_handleActions.TryGetValue(command.GetType(), out action))
            {
                throw new ApplicationException("Unknown command");
            }

            return action(command);
        }
    }
}