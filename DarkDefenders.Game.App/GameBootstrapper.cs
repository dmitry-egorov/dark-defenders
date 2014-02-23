using System;
using System.Collections.Generic;
using DarkDefenders.Game.App.Interfaces;
using DarkDefenders.Game.App.Internals;
using DarkDefenders.Game.Model;
using Infrastructure.DDDES;
using Infrastructure.Unity;
using Microsoft.Practices.Unity;

namespace DarkDefenders.Game.App
{
    public class GameBootstrapper: IUnityContainer
    {
        private readonly IUnityContainer _container = new UnityContainer();
        private readonly List<Type> _registeredListeners = new List<Type>();

        internal GameBootstrapper(IUnityContainer container)
        {
            _container = container;
        }

        public GameBootstrapper RegisterResource<TResource>(IResources<TResource> resource)
        {
            _container.RegisterInstance(resource);

            return this;
        }

        public GameBootstrapper RegisterListener<TEvents>(Func<TEvents> factoryFunc)
        {
            _container.RegisterType<TEvents>(new InjectionFactory(x => factoryFunc));

            _registeredListeners.Add(typeof(TEvents));

            return this;
        }

        public GameBootstrapper RegisterListener<TEvents, TListener>() 
            where TListener : TEvents
            where TEvents: IEntityEvents
        {
            _container.RegisterType<TEvents, TListener>();

            _registeredListeners.Add(typeof(TEvents));

            return this;
        }

        public GameBootstrapper RegisterGameService()
        {
            _container
                .RegisterDomain(_registeredListeners)
                .RegisterSingleton<GameService, IGameService>();

            return this;
        }

        public IGameService Bootstrap()
        {
            return _container.ResolveGame();
        }

        public GameBootstrapper RegisterSingleton<T>()
        {
            _container.RegisterSingleton<T>();
            return this;
        }

        public GameBootstrapper RegisterSingleton<T, TI>() 
            where T : TI
        {
            _container.RegisterSingleton<T, TI>();
            return this;
        }

        public IUnityContainer RegisterType(Type @from, Type to, string name, LifetimeManager lifetimeManager,
            params InjectionMember[] injectionMembers)
        {
            return _container.RegisterType(@from, to, name, lifetimeManager, injectionMembers);
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            return _container.RegisterInstance(t, name, instance, lifetime);
        }

        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            return _container.Resolve(t, name, resolverOverrides);
        }

        public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            return _container.ResolveAll(t, resolverOverrides);
        }

        public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            return _container.BuildUp(t, existing, name, resolverOverrides);
        }

        public void Teardown(object o)
        {
            _container.Teardown(o);
        }

        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            return _container.AddExtension(extension);
        }

        public object Configure(Type configurationInterface)
        {
            return _container.Configure(configurationInterface);
        }

        public IUnityContainer RemoveAllExtensions()
        {
            return _container.RemoveAllExtensions();
        }

        public IUnityContainer CreateChildContainer()
        {
            return _container.CreateChildContainer();
        }

        public IUnityContainer Parent
        {
            get { return _container.Parent; }
        }

        public IEnumerable<ContainerRegistration> Registrations
        {
            get { return _container.Registrations; }
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}