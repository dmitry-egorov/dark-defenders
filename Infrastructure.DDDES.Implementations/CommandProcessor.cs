using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Util;

namespace Infrastructure.DDDES.Implementations
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly Dictionary<Type, IRootCommmandProcessor> _containersTypeMap = new Dictionary<Type, IRootCommmandProcessor>();
        private readonly Dictionary<Type, IRootEventsApplier> _idTypeAppliersMap = new Dictionary<Type, IRootEventsApplier>();

        private readonly Queue<IEvent> _commitQueue = new Queue<IEvent>();

        private readonly IEventsLinstener _eventsLinstener;

        public CommandProcessor(IEventsLinstener eventsLinstener)
        {
            _eventsLinstener = eventsLinstener;
        }

        public void AddRepository<TRootId, TRoot, TRootEvent>(IRepository<TRootId, TRoot> repository)
            where TRootId : Identity
            where TRoot: class, IRoot<TRootEvent>
        {
            _containersTypeMap[typeof (TRoot)] = new RootCommmandProcessor<TRootId, TRoot, TRootEvent>(repository, _commitQueue);
            _idTypeAppliersMap[typeof (TRootId)] = new RootEventsApplier<TRootId, TRoot, TRootEvent>(repository);
        }

        public T Request<T, TRoot>(Identity id, Func<TRoot, T> request)
        {
            return _containersTypeMap[typeof(TRoot)].Request(id, request);
        }

        public void Process<TRoot>(Identity id, Func<TRoot, IEnumerable<IEvent>> command)
        {
            _containersTypeMap[typeof (TRoot)].Process(id, command);
        }

        public void ProcessAllImplementing<T>(Func<T, IEnumerable<IEvent>> command)
        {
            _containersTypeMap
            .Where(x => typeof(T).IsAssignableFrom(x.Key))
            .ForEach(x => x.Value.ProcessAllImplementing(command));
        }

        public void Commit()
        {
            var events = _commitQueue.DequeueAll().AsReadOnly();

            events
            .GroupBy(x => x.RootId.GetType())
            .ForEach(x => _idTypeAppliersMap[x.Key].Apply(x));

            _eventsLinstener.Apply(events);
        }

        public void Rollback()
        {
            _commitQueue.Clear();
        }

        private interface IRootCommmandProcessor
        {
            void Process<TRoot>(Identity id, Func<TRoot, IEnumerable<IEvent>> command);
            void ProcessAllImplementing<T>(Func<T, IEnumerable<IEvent>> command);
            T Request<T, TRoot>(Identity id, Func<TRoot, T> request);
        }

        private interface IRootEventsApplier
        {
            void Apply(IEnumerable<IEvent> events);
        }

        private class RootCommmandProcessor<TRootId, TRoot, TRootEvent>: IRootCommmandProcessor
            where TRootId: Identity
            where TRoot: IRoot<TRootEvent>
        {
            private readonly IRepository<TRootId, TRoot> _repository;
            private readonly Queue<IEvent> _queue;

            public RootCommmandProcessor(IRepository<TRootId, TRoot> repository, Queue<IEvent> queue)
            {
                _repository = repository;
                _queue = queue;
            }

            public void Process<TRequestedRoot>(Identity id, Func<TRequestedRoot, IEnumerable<IEvent>> command)
            {
                var root = GetRoot<TRequestedRoot>(id);

                var events = command(root);

                _queue.Enqueue(events);
            }

            public void ProcessAllImplementing<T>(Func<T, IEnumerable<IEvent>> command)
            {
                var roots = _repository.GetAll().Cast<T>();

                var events = roots.SelectMany(command);

                _queue.Enqueue(events);
            }

            public T Request<T, TRequestedRoot>(Identity id, Func<TRequestedRoot, T> request)
            {
                var root = GetRoot<TRequestedRoot>(id);

                return request(root);
            }

            private TRequestedRoot GetRoot<TRequestedRoot>(Identity id)
            {
                if (typeof (TRequestedRoot) != typeof (TRoot))
                {
                    throw new InvalidOperationException("Wrong type requested");
                }

                var rootOriginal = _repository.GetById((TRootId) id);
                return (TRequestedRoot) (object) rootOriginal;
            }
        }

        private class RootEventsApplier<TRootId, TRoot, TRootEvent> : IRootEventsApplier
            where TRootId: Identity
            where TRoot: IRoot<TRootEvent>
        {
            private readonly IRepository<TRootId, TRoot> _repository;

            public RootEventsApplier(IRepository<TRootId, TRoot> repository)
            {
                _repository = repository;
            }

            public void Apply(IEnumerable<IEvent> events)
            {
                events
                .GroupBy(x => x.RootId)
                .ForEach(x =>
                {
                    var root = _repository.GetById((TRootId)x.Key);
                    root.Apply(x.Cast<TRootEvent>());
                });
            }
        }
    }
}