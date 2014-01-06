using System;
using System.Collections.Generic;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.DDDES.Implementations.Domain.Exceptions;
using Infrastructure.DDDES.Implementations.Internal;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations
{
    public class CommandProcessor<TDomainEvent> : ICommandProcessor<TDomainEvent>
        where TDomainEvent: IEvent
    {
        private readonly Dictionary<Type, object> _containersTypeMap = new Dictionary<Type, object>();
        private readonly Dictionary<Type, IRootEventsApplier> _idTypeAppliersMap = new Dictionary<Type, IRootEventsApplier>();
        private readonly Dictionary<Type, object> _factories = new Dictionary<Type, object>();

        private readonly Queue<TDomainEvent> _commitQueue = new Queue<TDomainEvent>();

        private readonly IEventsListener<TDomainEvent> _eventsListener;

        public CommandProcessor(IEventsListener<TDomainEvent> eventsListener)
        {
            _eventsListener = eventsListener;
        }

        public void RegisterRoot<TRootId, TRoot, TRootEvent, TRootFactory, TCreatedEvent, TDestroyedEvent>(Repository<TRootId, TRoot> repository, TRootFactory factory)
            where TRootId : Identity
            where TRoot: class, IRoot<TRootId, TRootEvent> 
            where TRootFactory: IRootFactory<TRoot, TCreatedEvent>
            where TCreatedEvent : class where TRootEvent : class
        {
            _containersTypeMap[typeof(TRoot)] = new RootCommandProcessor<TRootId, TRoot, TRootEvent>(repository, _commitQueue);
            _idTypeAppliersMap[typeof(TRootId)] = new RootEventsApplier<TRootId, TRoot, TRootEvent, TCreatedEvent, TDestroyedEvent>(repository, factory);
            _factories[typeof (TRootFactory)] = factory;
        }

        public void CommitAll<TRoot>(Func<TRoot, IEnumerable<TDomainEvent>> command)
        {
            try
            {
                ProcessAll(command);
                Commit();
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        public void Create<TRootFactory>(Func<TRootFactory, IEnumerable<TDomainEvent>> creation)
        {
            var factory = (TRootFactory)_factories[typeof (TRootFactory)];

            var events = creation(factory);

            _commitQueue.Enqueue(events);
        }

        public void CommitCreation<TRootFactory>(Func<TRootFactory, IEnumerable<TDomainEvent>> creation)
        {
            try
            {
                Create(creation);
                Commit();
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        public void Process<TRoot>(Identity id, Func<TRoot, IEnumerable<TDomainEvent>> command)
        {
            var commmandProcessor = GetProcessorFor<TRoot>();

            commmandProcessor.Process(id, command);
        }

        public void Commit<TRoot>(Identity id, Func<TRoot, IEnumerable<TDomainEvent>> command)
        {
            try
            {
                Process(id, command);
                Commit();
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        public void ProcessAll<TRoot>(Func<TRoot, IEnumerable<TDomainEvent>> command)
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

            _eventsListener.Recieve(events);

            _commitQueue.Clear();
        }

        public void Rollback()
        {
            _commitQueue.Clear();
        }

        public IAllRootsAdapter<TRoot, TDomainEvent> CreateRootsAdapter<TRoot>()
        {
            var rootCommandProcessor = GetProcessorFor<TRoot>();

            return new AllRootsToProcessorAdapter<TRoot, TDomainEvent>(rootCommandProcessor, this);
        }

        public IRootAdapter<TRoot, TDomainEvent> CreateRootAdapter<TRoot>(Identity id)
        {
            var rootCommandProcessor = GetProcessorFor<TRoot>();

            return new RootToProcessorAdapter<TRoot, TDomainEvent>(id, rootCommandProcessor, this);
        }

        private IRootCommandProcessor<TRoot, TDomainEvent> GetProcessorFor<TRoot>()
        {
            return (IRootCommandProcessor<TRoot, TDomainEvent>)_containersTypeMap[typeof(TRoot)];
        }

        private interface IRootEventsApplier
        {
            void Apply(Identity key, IEnumerable<TDomainEvent> events);
        }

        private class RootCommandProcessor<TRootId, TRoot, TRootEvent> : IRootCommandProcessor<TRoot, TDomainEvent>
            where TRootId: Identity
            where TRoot: IRoot<TRootId, TRootEvent>
        {
            private readonly IRepository<TRootId, TRoot> _repository;
            private readonly Queue<TDomainEvent> _queue;

            public RootCommandProcessor(IRepository<TRootId, TRoot> repository, Queue<TDomainEvent> queue)
            {
                _repository = repository;
                _queue = queue;
            }

            public void Process(Identity id, Func<TRoot, IEnumerable<TDomainEvent>> command)
            {
                var root = _repository.GetById((TRootId) id);

                var events = command(root);

                _queue.Enqueue(events);
            }

            public void ProcessAll(Func<TRoot, IEnumerable<TDomainEvent>> command)
            {
                var roots = _repository.GetAll();

                foreach (var root in roots)
                {
                    var events = command(root);
                    _queue.Enqueue(events);
                }
            }
        }

        private class RootEventsApplier<TRootId, TRoot, TRootEvent, TCreatedEvent, TDestoyedEvent> : IRootEventsApplier
            where TRootId: Identity
            where TRoot: IRoot<TRootId, TRootEvent> 
            where TCreatedEvent : class 
            where TRootEvent : class
        {
            private readonly Repository<TRootId, TRoot> _repository;
            private readonly IRootFactory<TRoot, TCreatedEvent> _factory;

            public RootEventsApplier(Repository<TRootId, TRoot> repository, IRootFactory<TRoot, TCreatedEvent> factory)
            {
                _repository = repository;
                _factory = factory;
            }

            public void Apply(Identity key, IEnumerable<TDomainEvent> events)
            {
                Apply((TRootId)key, events);
            }

            private void Apply(TRootId rootId, IEnumerable<TDomainEvent> events)
            {
                TRoot root;
                if (!_repository.TryGetById(rootId, out root))
                {
                    TDomainEvent firstEvent;
                    events = events.HeadAndTail(out firstEvent);

                    var creationEvent = firstEvent as TCreatedEvent;

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
                    var e = enumerator.Current as TRootEvent;
                    if (e != null)
                    {
                        root.Apply(e);
                    }
                    else if (enumerator.Current.IsNot<TDestoyedEvent>())
                    {
                        throw new InvalidOperationException("Event doesn't implement " + typeof(TRootEvent).Name);
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