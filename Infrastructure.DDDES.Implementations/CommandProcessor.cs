using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly Dictionary<Type, Func<Func<object, IEnumerable<IEvent>>, IEnumerable<IEvent>>> _processAllFuncs = new Dictionary<Type, Func<Func<object, IEnumerable<IEvent>>, IEnumerable<IEvent>>>();
        private readonly Dictionary<Type, Func<Identity, Func<object, IEnumerable<IEvent>>, IEnumerable<IEvent>>> _processRootFuncs = new Dictionary<Type, Func<Identity, Func<object, IEnumerable<IEvent>>, IEnumerable<IEvent>>>();

        public void AddRepository<TRoot, TRootId, TRootEvent>(IRootsStorage<TRootId, TRoot, TRootEvent> repository)
            where TRootId : Identity
            where TRoot: class
        {
            _processAllFuncs[typeof(TRoot)] = command => ProcessAllEvents(repository, command);
            _processRootFuncs[typeof(TRoot)] = (id, command) => ProcessRootEvents(repository, id, command);
        }

        public IEnumerable<IEvent> Process<TRoot>(Identity id, Func<TRoot, IEnumerable<IEvent>> command)
        {
            var func = GetFunc<TRoot>();

            return func(id, c => command((TRoot)c));
        }

        public IEnumerable<IEvent> ProcessAllImplementing<TInterface>(Func<TInterface, IEnumerable<IEvent>> command)
        {
            return _processAllFuncs
                   .Where(x => typeof(TInterface).IsAssignableFrom(x.Key))
                   .SelectMany(x => x.Value(o => command((TInterface)o)));
        }

        private Func<Identity, Func<object, IEnumerable<IEvent>>, IEnumerable<IEvent>> GetFunc<TRoot>()
        {
            Func<Identity, Func<object, IEnumerable<IEvent>>, IEnumerable<IEvent>> func;
            if (!_processRootFuncs.TryGetValue(typeof (TRoot), out func))
            {
                throw new ApplicationException("Unknown command");
            }

            return func;
        }

        private static IEnumerable<IEvent> ProcessRootEvents<TRoot, TRootId, TRootEvent>(IRootsStorage<TRootId, TRoot, TRootEvent> repository, Identity id, Func<object, IEnumerable<IEvent>> command) 
            where TRootId : Identity 
        {
            var rootId = (TRootId) id;
            var root = repository.GetById(rootId);
            var events = command(root).AsReadOnly();
            return Apply(repository, events);
        }

        private static IEnumerable<IEvent> ProcessAllEvents<TRoot, TRootId, TRootEvent>(IRootsStorage<TRootId, TRoot, TRootEvent> repository, Func<object, IEnumerable<IEvent>> command)
            where TRootId : Identity where TRoot : class
        {
            var roots = repository.GetAll();
            var events = roots.SelectMany(root => command(root)).AsReadOnly();

            return Apply(repository, events);
        }

        private static IEnumerable<IEvent> Apply<TRoot, TRootId, TRootEvent>(IRootsStorage<TRootId, TRoot, TRootEvent> repository, ReadOnlyCollection<IEvent> events)
            where TRootId : Identity
        {
            var rootEvents = events.Cast<TRootEvent>();

            repository.Apply(rootEvents);

            return events;
        }
    }
}