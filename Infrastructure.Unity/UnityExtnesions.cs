using Microsoft.Practices.Unity;

namespace Infrastructure.Unity
{
    public static class UnityExtnesions
    {
        public static IUnityContainer RegisterSingleton<T>(this IUnityContainer container, params InjectionMember[] injectionMembers)
        {
            return container.RegisterType<T>(new ContainerControlledLifetimeManager(), injectionMembers);
        }

        public static IUnityContainer RegisterSingleton<T, TI>(this IUnityContainer container) 
            where T : TI
        {
            return container
                   .RegisterType<TI, T>(new ContainerControlledLifetimeManager());
        }

        public static IUnityContainer RegisterSingleton<T, TI1, TI2>(this IUnityContainer container) 
            where T : TI1, TI2
        {
            var result = container
                .RegisterType<TI1, T>(new ContainerControlledLifetimeManager())
                .RegisterType<TI2, T>(new ContainerControlledLifetimeManager());

            return result;
        }

        public static IUnityContainer RegisterSingleton<T, TI1, TI2, TI3>(this IUnityContainer container)
            where T : TI1, TI2, TI3
        {
            var result = container
                   .RegisterType<TI1, T>(new ContainerControlledLifetimeManager())
                   .RegisterType<TI2, T>(new ContainerControlledLifetimeManager())
                   .RegisterType<TI3, T>(new ContainerControlledLifetimeManager())
                   ;

            return result;
        }

        public static IUnityContainer RegisterSingleton<T>(this IUnityContainer container)
        {
            return container.RegisterType<T>(new ContainerControlledLifetimeManager());
        }
    }
}
