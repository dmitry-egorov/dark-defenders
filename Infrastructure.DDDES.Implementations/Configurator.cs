using System.Collections.Generic;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.DDDES.Implementations.Internals;
using Infrastructure.Unity;
using Microsoft.Practices.Unity;

namespace Infrastructure.DDDES.Implementations
{
    public static class Configurator
    {
        public static IUnityContainer RegisterProcessor<TReciever>(this IUnityContainer container)
        {
            return container.RegisterSingleton<EventsProcessor<TReciever>, IEventsProcessor>();
        }

        public static IUnityContainer RegisterSingletonEntity<T>(this IUnityContainer container)
            where T : class
        {
            return container
                .RegisterFactory<T>()
                .RegisterSingleton<T>()
                .RegisterSingleton<SingletonStorage<T>, IStorage<T>>()
                ;
        }

        public static IUnityContainer RegisterEntity<T>(this IUnityContainer container)
        {
            return container
                .RegisterFactory<T>()
                .RegisterSingleton<Repository<T>, IStorage<T>, IReadOnlyList<T>>();
        }

        private static IUnityContainer RegisterFactory<TEntity>(this IUnityContainer container)
        {
            return container
                .RegisterSingleton<IFactory<TEntity>>(new InjectionFactory(c => new UnityFactory<TEntity>(c)));
        }
    }
}