using System;
using System.Collections.Generic;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.DDDES.Implementations.Domain.Exceptions;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly Dictionary<Type, object> _containersTypeMap = new Dictionary<Type, object>();
        private readonly Dictionary<Type, IRootEventsApplier> _idTypeAppliersMap = new Dictionary<Type, IRootEventsApplier>();
        private readonly Dictionary<Type, object> _factories = new Dictionary<Type, object>();

        private readonly Queue<IEvent> _commitQueue = new Queue<IEvent>();

        private readonly IEventsLinstener _eventsLinstener;

        public CommandProcessor(IEventsLinstener eventsLinstener)
        {
            _eventsLinstener = eventsLinstener;
        }

        public void AddRepository<TRootId, TRoot, TRootEvent, TRootFactory, TCreatedEvent>(Repository<TRootId, TRoot> repository, TRootFactory factory)
            where TRootId : Identity
            where TRoot: class, IRoot<TRootId, TRootEvent> 
            where TRootFactory: IRootFactory<TRoot, TCreatedEvent>
            where TCreatedEvent : class where TRootEvent : class
        {
            _containersTypeMap[typeof (TRoot)] = new RootCommandProcessor<TRootId, TRoot, TRootEvent>(repository, _commitQueue);
            _idTypeAppliersMap[typeof(TRootId)] = new RootEventsApplier<TRootId, TRoot, TRootEvent, TCreatedEvent>(repository, factory);
            _factories[typeof (TRootFactory)] = factory;
        }

        public void Create<TRootFactory>(Func<TRootFactory, IEnumerable<IEvent>> creation)
        {
            var factory = (TRootFactory)_factories[typeof (TRootFactory)];

            var events = creation(factory);

            _commitQueue.Enqueue(events);
        }

        public void Process<TRoot>(Identity id, Func<TRoot, IEnumerable<IEvent>> command)
        {
            var commmandProcessor = GetProcessorFor<TRoot>();

            commmandProcessor.Process(id, command);
        }

        public void ProcessAll<TRoot>(Func<TRoot, IEnumerable<IEvent>> command)
        {
            var commmandProcessor = GetProcessorFor<TRoot>();

            commmandProcessor.ProcessAll(command);
        }

        public void Commit()
        {
            if (_commitQueue.Count == 0)
            {
                return;
            }

            var events = _commitQueue;

            foreach (var grouping in events.GroupAdjacentFast(x => x.RootId))
            {
                var id = grouping.Key;

                var applier = _idTypeAppliersMap[id.GetType()];

                applier.Apply(id, grouping);
            }

            _eventsLinstener.Recieve(events);

            _commitQueue.Clear();
        }

        public void Rollback()
        {
            _commitQueue.Clear();
        }

        public IRootCommandProcessor<TRoot> GetProcessorFor<TRoot>()
        {
            return (IRootCommandProcessor<TRoot>)_containersTypeMap[typeof(TRoot)];
        }

        private interface IRootEventsApplier
        {
            void Apply(Identity key, IEnumerable<IEvent> events);
        }

        private class RootCommandProcessor<TRootId, TRoot, TRootEvent>: IRootCommandProcessor<TRoot>
            where TRootId: Identity
            where TRoot: IRoot<TRootId, TRootEvent>
        {
            private readonly IRepository<TRootId, TRoot> _repository;
            private readonly Queue<IEvent> _queue;

            public RootCommandProcessor(IRepository<TRootId, TRoot> repository, Queue<IEvent> queue)
            {
                _repository = repository;
                _queue = queue;
            }

            public void Process(Identity id, Func<TRoot, IEnumerable<IEvent>> command)
            {
                var root = _repository.GetById((TRootId) id);

                var events = command(root);

                _queue.Enqueue(events);
            }

            public void ProcessAll(Func<TRoot, IEnumerable<IEvent>> command)
            {
                var roots = _repository.GetAll();

                foreach (var root in roots)
                {
                    var events = command(root);
                    _queue.Enqueue(events);
                }
            }
        }

        private class RootEventsApplier<TRootId, TRoot, TRootEvent, TCreationEvent> : IRootEventsApplier
            where TRootId: Identity
            where TRoot: IRoot<TRootId, TRootEvent> 
            where TCreationEvent : class where TRootEvent : class
        {
            private readonly Repository<TRootId, TRoot> _repository;
            private readonly IRootFactory<TRoot, TCreationEvent> _factory;

            public RootEventsApplier(Repository<TRootId, TRoot> repository, IRootFactory<TRoot, TCreationEvent> factory)
            {
                _repository = repository;
                _factory = factory;
            }

            public void Apply(Identity key, IEnumerable<IEvent> events)
            {
                Apply((TRootId)key, events);
            }

            private void Apply(TRootId rootId, IEnumerable<IEvent> events)
            {
                TRoot root;
                if (!_repository.TryGetById(rootId, out root))
                {
                    IEvent firstEvent;
                    events = events.HeadAndTail(out firstEvent);

                    var creationEvent = firstEvent as TCreationEvent;

                    if (creationEvent == null)
                    {
                        throw new RootDoesntExistException(typeof(TRoot).Name, rootId);
                    }

                    root = _factory.Handle(creationEvent);

                    _repository.Store(root);
                }

                using (var enumerator = events.GetEnumerator())
                while (enumerator.MoveNext())
                {
                    var @event = enumerator.Current as TRootEvent;
                    if (@event != null)
                    {
                        root.Apply(@event);
                    }
                    else
                    {
                        _repository.Remove(rootId);
                        if (enumerator.MoveNext())
                        {
                            throw new RootDoesntExistException(typeof(TRoot).Name, rootId);
                        }

                        break;
                    }
                }
            }
        }
    }
}