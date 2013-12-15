using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.DDDES.Implementations
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly Dictionary<Type, Func<IEnumerable<object>>> _getAllFuncs = new Dictionary<Type, Func<IEnumerable<object>>>();
        private readonly Dictionary<Type, Func<Identity, object>> _getByIdFuncs = new Dictionary<Type, Func<Identity, object>>();

        public void AddRepository<TRoot, TRootId>(IRepository<TRoot, TRootId> repository)
            where TRootId : Identity
            where TRoot: class
        {
            _getAllFuncs[typeof(TRoot)] = repository.GetAll;
            _getByIdFuncs[typeof(TRoot)] = id => repository.GetById((TRootId)id);
        }

        public IEnumerable<IEvent> Process<TRoot>(Identity id, Func<TRoot, IEnumerable<IEvent>> command)
        {
            var func = GetFunc<TRoot>();

            var root = (TRoot)func(id);

            return command(root);
        }

        public IEnumerable<IEvent> ProcessAllImplementing<T>(Func<T, IEnumerable<IEvent>> command)
        {
            return _getAllFuncs
                .Where(x => typeof(T).IsAssignableFrom(x.Key))
                .SelectMany(x => x.Value().SelectMany(obj => command((T)obj)));
        }

        private Func<Identity, object> GetFunc<TRoot>()
        {
            Func<Identity, object> func;
            if (!_getByIdFuncs.TryGetValue(typeof (TRoot), out func))
            {
                throw new ApplicationException("Unknown command");
            }

            return func;
        }
    }
}