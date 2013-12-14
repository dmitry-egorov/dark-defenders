using System;
using System.Collections.Generic;
using Infrastructure.DDDEventSourcing.Domain;
using Infrastructure.Util;

namespace Infrastructure.DDDEventSourcing.Implementations
{
    public class Bus: ICommandPublisher
    {
        private readonly Dictionary<Type, Func<ICommand, IEnumerable<IEvent>>> _handleActions = new Dictionary<Type, Func<ICommand, IEnumerable<IEvent>>>();

        private readonly IEventStore _eventStore;

        public Bus(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void Publish(ICommand command)
        {
            Func<ICommand, IEnumerable<IEvent>> action;
            if (!_handleActions.TryGetValue(command.GetType(), out action))
            {
                throw new ApplicationException("Unknown command");
            }

            var events = action(command);

            _eventStore.Append(command.AggregateRootId, events);
        }

        public void AddRepositoryFor<TCommand, TAggregateRoot, TIdentity>(IRepository<TAggregateRoot, TIdentity> repository)
            where TCommand: ICommand<TIdentity>
            where TAggregateRoot : ICommandHandler<TCommand>
            where TIdentity : Identity
        {
            _handleActions[typeof(TCommand)] = command => Handle(repository, (TCommand)command);
        }

        private static IEnumerable<IEvent> Handle<TCommand, TAggregateRoot, TIdentity>(IRepository<TAggregateRoot, TIdentity> repository, TCommand command)
            where TCommand : ICommand<TIdentity>
            where TAggregateRoot : ICommandHandler<TCommand>
            where TIdentity: Identity
        {
            var aggregateRoot = repository.GetById(command.AggregateRootId);

            return aggregateRoot.Handle(command).AsReadOnly();
        }
    }
}