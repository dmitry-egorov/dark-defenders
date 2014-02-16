using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.DDDES.Implementations.Internals;
using Infrastructure.Unity;
using Infrastructure.Util;
using Microsoft.Practices.Unity;
using NSubstitute;

namespace Infrastructure.DDDES.Implementations.Configuration
{
    public class DomainConfigurator
    {
        private readonly IUnityContainer _container;
        private readonly IEnumerable<Type> _registeredListeners;

        public DomainConfigurator(IUnityContainer container, IEnumerable<Type> registeredListeners)
        {
            _container = container;
            _registeredListeners = registeredListeners;

            RegisterProcessor();
        }

        public DomainConfigurator RegisterSingletonEntity<T>()
            where T : class
        {
            RegisterFactory<T>();
            RegisterListener<T>();
            RegisterSingleton<T>();
            _container.RegisterSingleton<SingletonStorage<T>, IStorage<T>>()
            ;

            return this;
        }

        public DomainConfigurator RegisterEntity<T>()
        {
            RegisterFactory<T>();
            RegisterListener<T>();
            _container.RegisterSingleton<Repository<T>, IStorage<T>, IReadOnlyList<T>>();

            return this;
        }

        public DomainConfigurator RegisterSingleton<T>()
        {
            _container.RegisterSingleton<T>();

            return this;
        }

        public DomainConfigurator RegisterSingleton<T>(params InjectionMember[] injectionMembers)
        {
            _container.RegisterSingleton<T>(injectionMembers);

            return this;
        }

        private void RegisterListener<TEntity>()
        {
            var type = typeof(TEntity);
            if (_registeredListeners.Any(x => x.IsAssignableFrom(type)))
            {
                return;
            }

            var baseInterface = typeof(IEntityEvents);

            var eventsInterface = type.GetInterfaces().Single(t => baseInterface.IsAssignableFrom(t) && t != baseInterface);

            var instance = Substitute.For(eventsInterface.EnumerateOnce().ToArray(), new object[0]);
            _container.RegisterInstance(eventsInterface, instance);
        }

        private void RegisterFactory<TEntity>()
        {
            _container
            .RegisterSingleton<IFactory<TEntity>>(new InjectionFactory(c => new UnityFactory<TEntity>(c)));
        }

        private void RegisterProcessor()
        {
            _container.RegisterSingleton<EventsProcessor, IEventsProcessor>();
        }
    }
}